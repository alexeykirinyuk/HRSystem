using System.Collections.Generic;
using HRSystem.Web.Dtos;

namespace HRSystem.Queries.GetEmployeeSavingInfo
{
    public class GetEmployeeSavingInfoQueryResponse
    {
        public EmployeeDto Employee { get; set; }
        public ICollection<AttributeInfoDto> Attributes { get; set; }
        public ICollection<EmployeeDto> Employees { get; set; }
    }
}