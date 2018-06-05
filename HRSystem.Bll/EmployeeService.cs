using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Schema;
using HRSystem.Bll.Extensions;
using HRSystem.Common.Errors;
using HRSystem.Core;
using HRSystem.Data;
using HRSystem.Domain;
using HRSystem.Domain.Attributes;
using HRSystem.Domain.Attributes.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace HRSystem.Bll
{
    public class EmployeeService : IEmployeeService
    {
        private readonly HrSystemDb _db;
        private readonly IAccountService _accountService;
        private readonly IDocumentService _documentService;

        public EmployeeService(HrSystemDb hrSystemDb, IAccountService accountService, IDocumentService documentService)
        {
            ArgumentHelper.EnsureNotNull(nameof(hrSystemDb), hrSystemDb);
            ArgumentHelper.EnsureNotNull(nameof(accountService), accountService);
            ArgumentHelper.EnsureNotNull(nameof(documentService), documentService);

            _db = hrSystemDb;
            _accountService = accountService;
            _documentService = documentService;
        }

        public async Task<IEnumerable<Employee>> GetAll()
        {
            var employeeArray = await _db.Employees
                .Include(e => e.Attributes)
                .Include(e => e.Manager)
                .ToArrayAsync()
                .ConfigureAwait(false);

            return WithDocuments(employeeArray);
        }

        public async Task<IEnumerable<Employee>> Search(
            string managerFullName,
            string office,
            string jobTitle,
            string allAttributes,
            Dictionary<int, string> attributeFilters)
        {
            var employees = _db.Employees.AsQueryable();
            if (!string.IsNullOrEmpty(allAttributes))
            {
                employees = employees.Search(allAttributes);
            }

            if (!string.IsNullOrEmpty(managerFullName))
            {
                employees = employees.SearchByManager(managerFullName);
            }

            if (!string.IsNullOrEmpty(office))
            {
                employees = employees.SearchByOffice(office);
            }

            if (!string.IsNullOrEmpty(jobTitle))
            {
                employees = employees.SearchByJobTitle(jobTitle);
            }

            foreach (var attributeFilter in attributeFilters)
            {
                employees = employees.SearchByAttribute(attributeFilter.Key, attributeFilter.Value);
            }

            var employeeArray = await employees
                .IncludeAll()
                .ToArrayAsync()
                .ConfigureAwait(false);

            return WithDocuments(employeeArray);
        }

        private IEnumerable<Employee> WithDocuments(Employee[] employeeArray)
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

        public async Task Update(Employee employee)
        {
            var employeeEntity = await _db.Employees.Include(e => e.Manager).Include(e => e.Attributes)
                .GetByLogin(employee.Login);
            employeeEntity.Update(employee);
            await _db.SaveChangesAsync();
        }

        public Task<bool> IsExists(string login)
        {
            return _db.Employees.AnyAsync(e => e.Login == login);
        }

        public async Task<Employee> GetByLogin(string login)
        {
            var employee = await _db.Employees
                .Include(e => e.Manager)
                .Include(e => e.Attributes)
                .SingleOrDefaultAsync(e => e.Login == login);
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
            var employeesLastUpdated =
                await _db.Employees.GetEmployeesUpdatedFrom(fromDate).IncludeAll().ToArrayAsync();
            var usersLastUpdated = _accountService.GetUsersUpdatedFrom(fromDate).ToArray();

            SyncEmployeesToUsers(employeesLastUpdated);
            await SyncUsersToEmployees(usersLastUpdated, employeesLastUpdated);
        }

        public async Task<IEnumerable<string>> GetOffices()
        {
            return await _db.Employees
                .Select(e => e.Office)
                .Distinct()
                .ToArrayAsync()
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<string>> GetJobTitles()
        {
            return await _db.Employees
                .Select(e => e.JobTitle)
                .Distinct()
                .ToArrayAsync()
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<string>> GetManagers()
        {
            return await _db.Employees
                .Where(e => e.Manager != null)
                .Select(e => e.Manager.FullName)
                .Distinct()
                .ToArrayAsync()
                .ConfigureAwait(false);
        }

        private void SyncEmployeesToUsers(IEnumerable<Employee> lastUpdatedEmployees)
        {
            var lastUpdatedEmployeeArray = lastUpdatedEmployees as Employee[] ?? lastUpdatedEmployees.ToArray();
            foreach (var lastUpdatedEmployee in lastUpdatedEmployeeArray)
            {
                var user = _accountService.GetByLogin(lastUpdatedEmployee.Login);
                if (user == null)
                {
                    CreateUser(lastUpdatedEmployee, lastUpdatedEmployeeArray);
                }
                else
                {
                    Account manager = null;
                    if (lastUpdatedEmployee.Manager != null)
                    {
                        manager = _accountService.GetByLogin(lastUpdatedEmployee.ManagerLogin);
                    }

                    _accountService.Update(lastUpdatedEmployee.ToAccount(manager?.DistinguishedName));
                }
            }
        }

        private void CreateUser(Employee lastUpdatedEmployee, IEnumerable<Employee> employeesLastUpdated)
        {
            var lastUpdatedEmployeeArray = employeesLastUpdated as Employee[] ?? employeesLastUpdated.ToArray();
            Account manager = null;
            if (lastUpdatedEmployee.Manager != null)
            {
                manager = _accountService.GetByLogin(lastUpdatedEmployee.ManagerLogin);
            }

            if (lastUpdatedEmployee.Manager != null &&
                manager == null &&
                lastUpdatedEmployeeArray.ContainsByLogin(lastUpdatedEmployee.ManagerLogin))
            {
                CreateUser(lastUpdatedEmployee.Manager, lastUpdatedEmployeeArray);
                manager = _accountService.GetByLogin(lastUpdatedEmployee.ManagerLogin);
            }

            _accountService.Create(lastUpdatedEmployee.ToAccount(manager?.DistinguishedName));
        }

        private async Task SyncUsersToEmployees(IEnumerable<Account> usersLastUpdated,
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
                    await Update(new Employee
                    {
                        Login = employee.Login,
                        FirstName = userLastUpdated.FirstName,
                        LastName = userLastUpdated.LastName,
                        Email = userLastUpdated.Email,
                        Phone = userLastUpdated.Phone,
                        JobTitle = userLastUpdated.JobTitle,
                        Office = userLastUpdated.Office,
                        ManagerLogin = userLastUpdated.Manager?.Login,
                        Attributes = employee.Attributes
                    });
                }
            }
        }
    }
}