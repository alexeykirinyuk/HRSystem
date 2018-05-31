using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRSystem.Bll.Extensions;
using HRSystem.Common.Errors;
using HRSystem.Core;
using HRSystem.Data;
using HRSystem.Domain;
using HRSystem.Domain.Attributes.Base;
using Microsoft.EntityFrameworkCore;

namespace HRSystem.Bll
{
    public class EmployeeService : IEmployeeService
    {
        private readonly HrSystemDb _db;
        private readonly IUserService _userService;

        public EmployeeService(HrSystemDb hrSystemDb, IUserService userService)
        {
            ArgumentHelper.EnsureNotNull(nameof(hrSystemDb), hrSystemDb);
            ArgumentHelper.EnsureNotNull(nameof(userService), userService);

            _db = hrSystemDb;
            _userService = userService;
        }

        public async Task<IEnumerable<Employee>> GetAll()
        {
            return await _db.Employees
                .Include(e => e.Attributes)
                .Include(e => e.Manager)
                .ToArrayAsync()
                .ConfigureAwait(false);
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

            var employee = await _db.Employees.Include(e => e.Attributes).Include(e => e.Manager).GetByLogin(login);
            employee.Update(
                firstName: firstName,
                lastName: lastName,
                email: email,
                phone: phone,
                jobTitle: jobTitle,
                office: office,
                managerLogin: managerLogin,
                attributes: attributes);
            employee.LastModified = DateTime.UtcNow;
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

        public Task<Employee> GetByLogin(string login)
        {
            return _db.Employees.SingleOrDefaultAsync(e => e.Login == login);
        }

        public async Task SyncWithActiveDirectory(DateTime fromDate)
        {
            var employeesLastUpdated = (await GetEmployeesUpdatedBetweenDates(fromDate)).ToArray();
            var usersLastUpdated = _userService.GetUsersUpdatedFrom(fromDate).ToArray();

            SyncEmployeesToUsers(employeesLastUpdated);
            await SyncUsersToEmployees(usersLastUpdated, employeesLastUpdated);
        }

        private async Task<IEnumerable<Employee>> GetEmployeesUpdatedBetweenDates(DateTime @from)
        {
            return await _db.Employees.Where(e => e.LastModified > from)
                .Include(e => e.Manager)
                .Include(e => e.Attributes)
                .ToArrayAsync()
                .ConfigureAwait(false);
        }

        private void SyncEmployeesToUsers(IEnumerable<Employee> employeesLastUpdated)
        {
            foreach (var lastUpdatedEmployee in employeesLastUpdated)
            {
                var user = _userService.GetByLogin(lastUpdatedEmployee.Login);
                if (user == null)
                {
                    User manager = null;
                    if (lastUpdatedEmployee.Manager != null)
                    {
                        manager = _userService.GetByLogin(lastUpdatedEmployee.ManagerLogin);
                    }

                    _userService.Create(lastUpdatedEmployee.ToUser(manager?.DistinguishedName));
                }else
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