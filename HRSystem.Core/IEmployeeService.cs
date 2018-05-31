using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRSystem.Domain;
using HRSystem.Domain.Attributes.Base;

namespace HRSystem.Core
{
    public interface IEmployeeService
    {
        Task<IEnumerable<Employee>> GetAll();

        Task Add(Employee employee);

        Task Update(
            string login,
            string firstName,
            string lastName,
            string email,
            string phone,
            string jobTitle,
            string office,
            string managerLogin,
            List<AttributeBase> attributes);
        
        Task<IEnumerable<AttributeInfo>> GetAttributes();
        
        Task<bool> IsExists(string login);
        
        Task<Employee> GetByLogin(string login);

        Task SyncWithActiveDirectory(DateTime fromDate);
    }
}