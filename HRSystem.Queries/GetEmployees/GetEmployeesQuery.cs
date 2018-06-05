using System.Collections.Generic;
using MediatR;

namespace HRSystem.Queries.GetEmployees
{
    public class GetEmployeesQuery : IRequest<GetEmployeesQueryResponse>
    {
        public string AllAttributesFilter { get; set; }
        
        public string ManagerFullNameFilter { get; set; }
        
        public string OfficeFilter { get; set; }
        
        public string JobTitleFilter { get; set; }
        
        public Dictionary<int, string> AttributeFilters { get; set; }
    }
}