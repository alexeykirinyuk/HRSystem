using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
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
            var attributes = request.Attributes
                .Select(a => _createAttributeService.CreateAttribute(a.AttributeInfoId, a.Value, a.Type)).ToList();
            
            if (request.IsCreateCommand)
            {
                var employee = new Employee
                {
                    Login = request.Login,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    ManagerLogin = request.ManagerLogin,
                    Office = request.Office,
                    Phone = request.Phone,
                    JobTitle = request.JobTitle,
                    Attributes = attributes
                };

                await _employeeService.Add(employee);
            }
            else
            {
                await _employeeService.Update(
                    login: request.Login,
                    firstName: request.FirstName,
                    lastName: request.LastName,
                    email: request.Email,
                    phone: request.Phone,
                    jobTitle: request.JobTitle,
                    office: request.Office,
                    managerLogin: request.ManagerLogin,
                    attributes: attributes).ConfigureAwait(false);
            }
        }
    }
}