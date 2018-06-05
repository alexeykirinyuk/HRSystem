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
        private readonly IAttributeInfoService _attributeInfoService;

        public GetEmployeeQueryHandler(
            IEmployeeService employeeService,
            IAttributeInfoService attributeInfoService)
        {
            ArgumentHelper.EnsureNotNull(nameof(employeeService), employeeService);
            ArgumentHelper.EnsureNotNull(nameof(attributeInfoService), attributeInfoService);

            _employeeService = employeeService;
            _attributeInfoService = attributeInfoService;
        }

        public async Task<GetEmployeesQueryResponse> Handle(GetEmployeesQuery request,
            CancellationToken cancellationToken)
        {
            IEnumerable<Employee> employeeEnumerable;
            if (!string.IsNullOrEmpty(request.AllAttributesFilter) ||
                !string.IsNullOrEmpty(request.ManagerFullNameFilter) || 
                !string.IsNullOrEmpty(request.OfficeFilter) ||
                !string.IsNullOrEmpty(request.JobTitleFilter) ||
                !string.IsNullOrEmpty(request.AllAttributesFilter) ||
                request.AttributeFilters.Any())
            {
                employeeEnumerable = await _employeeService.Search(
                    request.ManagerFullNameFilter,
                    request.OfficeFilter,
                    request.JobTitleFilter,
                    request.AllAttributesFilter,
                    request.AttributeFilters);
            }
            else
            {
                employeeEnumerable = await _employeeService.GetAll();
            }

            var attributes = await _attributeInfoService.GetAll().ConfigureAwait(false);
            var jobTitles = await _employeeService.GetJobTitles().ConfigureAwait(false);
            var managers = await _employeeService.GetManagers().ConfigureAwait(false);
            var offices = await _employeeService.GetOffices().ConfigureAwait(false);

            return new GetEmployeesQueryResponse
            {
                Employees = Mapper.Map<ICollection<EmployeeDto>>(employeeEnumerable.ToArray()),
                Attributes = Mapper.Map<ICollection<AttributeInfoDto>>(attributes.ToArray()),
                JobTitles = jobTitles.ToArray(),
                ManagerNames = managers.ToArray(),
                Offices = offices.ToArray()
            };
        }
    }
}