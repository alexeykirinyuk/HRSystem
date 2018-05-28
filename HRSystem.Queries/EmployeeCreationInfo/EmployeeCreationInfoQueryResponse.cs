using System.Collections.Generic;
using HRSystem.Domain;
using HRSystem.Domain.Attributes.Base;

namespace HRSystem.Queries.EmployeeCreationInfo
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class EmployeeCreationInfoQueryResponse
    {
        public ICollection<AttributeInfo> Attributes { get; set; }
        
        public ICollection<Employee> Employees { get; set; }
    }
}