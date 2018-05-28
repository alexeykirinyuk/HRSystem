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
        
        Task<IEnumerable<AttributeInfo>> GetAttributes();
        
        Task<bool> IsExists(string login);
    }
}