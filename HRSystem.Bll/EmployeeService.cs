using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRSystem.Common.Errors;
using HRSystem.Common.Validation;
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

        public EmployeeService(HrSystemDb hrSystemDb)
        {
            ArgumentHelper.EnsureNotNull(nameof(hrSystemDb), hrSystemDb);

            _db = hrSystemDb;
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
            return _db.Employees.SingleAsync(e => e.Login == login);
        }

        public Task SyncWithActiveDirectory()
        {
            
        }
    }
}