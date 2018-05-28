using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HRSystem.Common.Errors;
using HRSystem.Core;
using MediatR;

namespace HRSystem.Queries.EmployeeCreationInfo
{
    public class EmployeeCreationInfoQueryHandler : IRequestHandler<EmployeeCreationInfoQuery, EmployeeCreationInfoQueryResponse>
    {
        private readonly IEmployeeService _employeeService;
        private readonly IAttributeService _attributeService;

        public EmployeeCreationInfoQueryHandler(IEmployeeService employeeService, IAttributeService attributeService)
        {
            ArgumentHelper.EnsureNotNull(nameof(employeeService), employeeService);
            ArgumentHelper.EnsureNotNull(nameof(attributeService), attributeService);

            _employeeService = employeeService;
            _attributeService = attributeService;
        }

        public async Task<EmployeeCreationInfoQueryResponse> Handle(EmployeeCreationInfoQuery request, CancellationToken cancellationToken)
        {
            var attributes = await _attributeService.GetAll().ConfigureAwait(false);
            var employees = await _employeeService.GetAll().ConfigureAwait(false);
            
            return new EmployeeCreationInfoQueryResponse
            {
                Attributes = attributes.ToArray(),
                Employees = employees.ToArray()
            };
        }
    }
}