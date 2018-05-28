using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRSystem.Common.Errors;
using HRSystem.Common.Validation;
using HRSystem.Core;
using HRSystem.Global.Validation;

namespace HRSystem.Commands.SaveEmployee
{
    public class SaveEmployeeCommandValidator : IValidator<SaveEmployeeCommand>
    {
        private readonly IEmployeeService _employeeService;
        private readonly IAttributeService _attributeService;

        public SaveEmployeeCommandValidator(IEmployeeService employeeService, IAttributeService attributeService)
        {
            ArgumentHelper.EnsureNotNull(nameof(employeeService), employeeService);
            ArgumentHelper.EnsureNotNull(nameof(attributeService), attributeService);

            _employeeService = employeeService;
            _attributeService = attributeService;
        }

        public async Task Validate(List<ValidationFailure> list, SaveEmployee.SaveEmployeeCommand request)
        {
            list.NotNullOrEmpty("Login", request.Login);
            list.NotNullOrEmpty("FirstName", request.FirstName);
            list.NotNullOrEmpty("LastName", request.LastName);

            var tasks = new List<Task>
            {
                CheckLogin(list, request), 
                CheckManager(list, request),
                CheckAttributes(list, request)
            };

            foreach (var task in tasks)
            {
                await task.ConfigureAwait(false);
            }
        }

        private async Task CheckLogin(List<ValidationFailure> list, SaveEmployee.SaveEmployeeCommand request)
        {
            if (await _employeeService.IsExists(request.Login).ConfigureAwait(false))
            {
                list.Add("Same login already exists.");
            }
        }

        private async Task CheckManager(List<ValidationFailure> list, SaveEmployee.SaveEmployeeCommand request)
        {
            if (!await _employeeService.IsExists(request.ManagerLogin).ConfigureAwait(false))
            {
                list.Add("Manager login was't found.");
            }
        }

        private async Task CheckAttributes(List<ValidationFailure> list, SaveEmployee.SaveEmployeeCommand request)
        {
            var tasks = request.Attributes.Select(employeeAttribute => CheckAttribute(list, employeeAttribute))
                .ToList();
            foreach (var task in tasks)
            {
                await task.ConfigureAwait(false);
            }
        }

        private async Task CheckAttribute(List<ValidationFailure> list, EmployeeAttribute attribute)
        {
            if (!await _attributeService.IsExists(attribute.AttributeInfoId))
            {
                list.Add("Attribute with same id wasn't found.");
            }
        }
    }
}