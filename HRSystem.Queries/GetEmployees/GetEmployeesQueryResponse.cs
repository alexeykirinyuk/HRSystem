using System.Collections.Generic;
using HRSystem.Web.Dtos;

namespace HRSystem.Queries.GetEmployees
{
    public class GetEmployeesQueryResponse
    {
        public ICollection<EmployeeDto> Employees { get; set; }
        
        public ICollection<AttributeInfoDto> Attributes { get; set; }
    }
}