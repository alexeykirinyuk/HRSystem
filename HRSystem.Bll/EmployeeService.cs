using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRSystem.Bll.Extensions;
using HRSystem.Common.Errors;
using HRSystem.Core;
using HRSystem.Data;
using HRSystem.Domain;
using HRSystem.Domain.Attributes;
using HRSystem.Domain.Attributes.Base;
using Microsoft.EntityFrameworkCore;

namespace HRSystem.Bll
{
    public class EmployeeService : IEmployeeService
    {
        private readonly HrSystemDb _db;
        private readonly IUserService _userService;
        private readonly IDocumentService _documentService;

        public EmployeeService(HrSystemDb hrSystemDb, IUserService userService, IDocumentService documentService)
        {
            ArgumentHelper.EnsureNotNull(nameof(hrSystemDb), hrSystemDb);
            ArgumentHelper.EnsureNotNull(nameof(userService), userService);
            ArgumentHelper.EnsureNotNull(nameof(documentService), documentService);

            _db = hrSystemDb;
            _userService = userService;
            _documentService = documentService;
        }

        public async Task<IEnumerable<Employee>> GetAll()
        {
            return await WithDocuments(_db.Employees.IncludeAll()).ToArrayAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<Employee>> Search(string searchFilter)
        {
            var employeeQueryable = _db.Employees
                .IncludeAll()
                .Where(e => e.ContainsText(searchFilter));

            return await WithDocuments(employeeQueryable).ToArrayAsync().ConfigureAwait(false);
        }

        private IQueryable<Employee> WithDocuments(IQueryable<Employee> employeeArray)
        {
            var documentAttributeInfos = _db.AttributeInfos
                .Where(a => a.Type == AttributeType.Document);

            foreach (var employee in employeeArray)
            {
                foreach (var documentAttributeInfo in documentAttributeInfos)
                {
                    var exists = _documentService.IsExists(employee, documentAttributeInfo);
                    employee.Attributes.Add(new DocumentAttribute(exists, employee, documentAttributeInfo));
                }
            }

            return employeeArray;
        }

        public async Task Add(Employee employee)
        {
            ArgumentHelper.EnsureNotNull(nameof(employee), employee);

            employee.LastModified = DateTime.UtcNow;
            await _db.Employees.AddAsync(employee).ConfigureAwait(false);
            await _db.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task Update(
            string login,
            string firstName,
            string lastName,
            string email,
            string phone,
            string jobTitle,
            string office,
            string managerLogin,
            List<AttributeBase> attributes)
        {
            ArgumentHelper.EnsureNotNullOrEmpty(nameof(login), login);
            ArgumentHelper.EnsureNotNullOrEmpty(nameof(firstName), firstName);
            ArgumentHelper.EnsureNotNullOrEmpty(nameof(lastName), lastName);

            var employee = await _db.Employees.IncludeAll().GetByLogin(login);
            employee.Update(
                firstName: firstName,
                lastName: lastName,
                email: email,
                phone: phone,
                jobTitle: jobTitle,
                office: office,
                managerLogin: managerLogin,
                attributes: attributes);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<AttributeInfo>> GetAttributes()
        {
            return await _db.AttributeInfos.ToArrayAsync().ConfigureAwait(false);
        }

        public Task<bool> IsExists(string login)
        {
            return _db.Employees.AnyAsync(e => e.Login == login);
        }

        public async Task<Employee> GetByLogin(string login)
        {
            var employee = await _db.Employees.IncludeAll().SingleOrDefaultAsync(e => e.Login == login);
            var docAttrs = _db.AttributeInfos.Where(a => a.Type == AttributeType.Document);
            foreach (var attributeInfo in docAttrs)
            {
                var exists = _documentService.IsExists(employee, attributeInfo);
                employee.Attributes.Add(new DocumentAttribute(exists, employee, attributeInfo));
            }

            return employee;
        }

        public async Task SyncWithActiveDirectory(DateTime fromDate)
        {
            var employeesLastUpdated = GetEmployeesUpdatedBetweenDates(fromDate).ToArray();
            var usersLastUpdated = _userService.GetUsersUpdatedFrom(fromDate).ToArray();

            SyncEmployeesToUsers(employeesLastUpdated);
            await SyncUsersToEmployees(usersLastUpdated, employeesLastUpdated);
        }

        private IQueryable<Employee> GetEmployeesUpdatedBetweenDates(DateTime @from)
        {
            return _db.Employees.Where(e => e.LastModified > from)
                .Include(e => e.Manager)
                .Include(e => e.Attributes);
        }

        private void SyncEmployeesToUsers(IEnumerable<Employee> employeesLastUpdated)
        {
            foreach (var lastUpdatedEmployee in employeesLastUpdated)
            {
                var user = _userService.GetByLogin(lastUpdatedEmployee.Login);
                if (user == null)
                {
                    CreateUser(lastUpdatedEmployee, employeesLastUpdated);
                }
                else
                {
                    User manager = null;
                    if (lastUpdatedEmployee.Manager != null)
                    {
                        manager = _userService.GetByLogin(lastUpdatedEmployee.ManagerLogin);
                    }

                    _userService.Update(lastUpdatedEmployee.ToUser(manager?.DistinguishedName));
                }
            }
        }

        private void CreateUser(Employee lastUpdatedEmployee, IEnumerable<Employee> employeesLastUpdated)
        {
            var lastUpdatedEmployeeArray = employeesLastUpdated as Employee[] ?? employeesLastUpdated.ToArray();
            User manager = null;
            if (lastUpdatedEmployee.Manager != null)
            {
                manager = _userService.GetByLogin(lastUpdatedEmployee.ManagerLogin);
            }

            if (lastUpdatedEmployee.Manager != null &&
                manager == null &&
                lastUpdatedEmployeeArray.ContainsByLogin(lastUpdatedEmployee.ManagerLogin))
            {
                CreateUser(lastUpdatedEmployee.Manager, lastUpdatedEmployeeArray);
                manager = _userService.GetByLogin(lastUpdatedEmployee.ManagerLogin);
            }

            _userService.Create(lastUpdatedEmployee.ToUser(manager?.DistinguishedName));
        }

        private async Task SyncUsersToEmployees(IEnumerable<User> usersLastUpdated,
            IEnumerable<Employee> employeesLastUpdated)
        {
            var lastUpdated = employeesLastUpdated as Employee[] ?? employeesLastUpdated.ToArray();

            foreach (var userLastUpdated in usersLastUpdated)
            {
                var employee = await GetByLogin(userLastUpdated.Login).ConfigureAwait(false);
                if (employee == null)
                {
                    await Add(userLastUpdated.ToEmployee()).ConfigureAwait(false);
                }
                else if (!lastUpdated.ContainsByLogin(userLastUpdated.Login))
                {
                    await Update(
                        login: employee.Login,
                        firstName: userLastUpdated.FirstName,
                        lastName: userLastUpdated.LastName,
                        email: userLastUpdated.Email,
                        phone: userLastUpdated.Phone,
                        jobTitle: userLastUpdated.JobTitle,
                        office: userLastUpdated.Office,
                        managerLogin: userLastUpdated.Manager?.Login,
                        attributes: employee.Attributes);
                }
            }
        }
    }
}