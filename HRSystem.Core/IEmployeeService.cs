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

        Task<IEnumerable<Employee>> Search(string searchFilter);
        
        Task Add(Employee employee);

        Task Update(Employee employee);
        
        Task<bool> IsExists(string login);
        
        Task<Employee> GetByLogin(string login);

        Task SyncWithActiveDirectory(DateTime fromDate);
    }
}