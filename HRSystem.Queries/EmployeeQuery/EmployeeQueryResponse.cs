using System.Collections.Generic;
using HRSystem.Domain;
using HRSystem.Domain.Attributes.Base;

namespace HRSystem.Queries.EmployeeQuery
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class EmployeeQueryResponse
    {
        public ICollection<Employee> Employees { get; set; }
        
        public ICollection<AttributeInfo> Attributes { get; set; }
    }
}