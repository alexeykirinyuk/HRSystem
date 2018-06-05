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

        Task<IEnumerable<Employee>> Search(
            string managerFullName,
            string office,
            string jobTitle,
            string allAttributes,
            Dictionary<int, string> attributeFilters);

        Task Add(Employee employee);

        Task Update(Employee employee);

        Task<bool> IsExists(string login);

        Task<Employee> GetByLogin(string login);

        Task SyncWithActiveDirectory(DateTime fromDate);

        Task<IEnumerable<string>> GetOffices();

        Task<IEnumerable<string>> GetJobTitles();

        Task<IEnumerable<string>> GetManagers();
    }
}