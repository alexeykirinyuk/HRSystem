using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HRSystem.Common.Errors;
using HRSystem.Core;
using HRSystem.Domain;
using MediatR;

namespace HRSystem.Commands.SaveEmployee
{
    public class SaveEmployeeCommandHandler : IRequestHandler<SaveEmployeeCommand>
    {
        private readonly IEmployeeService _employeeService;
        private readonly ICreateAttributeService _createAttributeService;

        public SaveEmployeeCommandHandler(IEmployeeService employeeService, ICreateAttributeService createAttributeService)
        {
            ArgumentHelper.EnsureNotNull(nameof(employeeService), employeeService);
            ArgumentHelper.EnsureNotNull(nameof(createAttributeService), createAttributeService);

            _employeeService = employeeService;
            _createAttributeService = createAttributeService;
        }

        public async Task Handle(SaveEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = new Employee(request.Login)
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                ManagerLogin = request.ManagerLogin,
                Phone = request.Phone,
                JobTitle = request.JobTitle,
                Attributes = request.Attributes
                    .Select(a => _createAttributeService.CreateAttribute(a.AttributeInfoId, a.Value, a.Type)).ToList()
            };

            await _employeeService.Add(employee);
        }
    }
}