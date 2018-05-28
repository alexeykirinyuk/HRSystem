using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly HrSystemDb _database;

        public EmployeeService(HrSystemDb hrSystemDb)
        {
            ArgumentHelper.EnsureNotNull(nameof(hrSystemDb), hrSystemDb);

            _database = hrSystemDb;
        }

        public async Task<IEnumerable<Employee>> GetAll()
        {
            return await _database.Employees
                .Include(e => e.Attributes)
                .Include(e => e.Manager)
                .ToArrayAsync()
                .ConfigureAwait(false);
        }

        public async Task Add(Employee employee)
        {
            ArgumentHelper.EnsureNotNull(nameof(employee), employee);

            await _database.Employees.AddAsync(employee).ConfigureAwait(false);
            await _database.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<AttributeInfo>> GetAttributes()
        {
            return await _database.AttributeInfos.ToArrayAsync().ConfigureAwait(false);
        }

        public async Task<bool> IsExists(string login)
        {
            return await _database.Employees.AnyAsync(e => e.Login == login).ConfigureAwait(false);
        }
    }
}