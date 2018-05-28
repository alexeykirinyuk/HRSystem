using System.Collections.Generic;
using HRSystem.Domain.Attributes.Base;
using HRSystem.Web.Dtos;

namespace HRSystem.Queries.EmployeeQuery
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class EmployeeQueryResponse
    {
        public ICollection<EmployeeDto> Employees { get; set; }
        
        public ICollection<AttributeInfoDto> Attributes { get; set; }
    }
}