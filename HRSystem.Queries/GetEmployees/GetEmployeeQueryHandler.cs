using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HRSystem.Common.Errors;
using HRSystem.Core;
using HRSystem.Domain;
using HRSystem.Web.Dtos;
using MediatR;

namespace HRSystem.Queries.GetEmployees
{
    public class GetEmployeeQueryHandler : IRequestHandler<GetEmployeesQuery, GetEmployeesQueryResponse>
    {
        private readonly IEmployeeService _employeeService;
        private readonly IAttributeService _attributeService;

        public GetEmployeeQueryHandler(
            IEmployeeService employeeService,
            IAttributeService attributeService)
        {
            ArgumentHelper.EnsureNotNull(nameof(employeeService), employeeService);
            ArgumentHelper.EnsureNotNull(nameof(attributeService), attributeService);

            _employeeService = employeeService;
            _attributeService = attributeService;
        }

        public async Task<GetEmployeesQueryResponse> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Employee> employeeEnumerable;
            if (!string.IsNullOrEmpty(request.SearchFilter))
            {
                employeeEnumerable = await _employeeService.Search(request.SearchFilter);
            }
            else
            {
                employeeEnumerable = await _employeeService.GetAll();
            }
            
            var attributes = await _attributeService.GetAll().ConfigureAwait(false);

            return new GetEmployeesQueryResponse
            {
                Employees = Mapper.Map<ICollection<EmployeeDto>>(employeeEnumerable.ToArray()),
                Attributes = Mapper.Map<ICollection<AttributeInfoDto>>(attributes.ToArray())
            };
        }
    }
}