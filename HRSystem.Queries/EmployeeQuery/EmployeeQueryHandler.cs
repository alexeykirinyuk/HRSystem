using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HRSystem.Common.Errors;
using HRSystem.Core;
using MediatR;

namespace HRSystem.Queries.EmployeeQuery
{
    public class EmployeeQueryHandler : IRequestHandler<EmployeeQuery, EmployeeQueryResponse>
    {
        private readonly IEmployeeService _employeeService;
        private readonly IAttributeService _attributeService;

        public EmployeeQueryHandler(
            IEmployeeService employeeService,
            IAttributeService attributeService)
        {
            ArgumentHelper.EnsureNotNull(nameof(employeeService), employeeService);
            ArgumentHelper.EnsureNotNull(nameof(attributeService), attributeService);

            _employeeService = employeeService;
            _attributeService = attributeService;
        }

        public async Task<EmployeeQueryResponse> Handle(EmployeeQuery request, CancellationToken cancellationToken)
        {
            var employees = await _employeeService.GetAll();
            var attributes = await _attributeService.GetAll().ConfigureAwait(false);

            return new EmployeeQueryResponse
            {
                Employees = employees.ToArray(),
                Attributes = attributes.ToArray()
            };
        }
    }
}