using System.Collections.Generic;
using System.Threading.Tasks;
using HRSystem.Common.Validation;
using HRSystem.Core;
using HRSystem.Global.Validation;

namespace HRSystem.Commands.AddDocument
{
    public class AddDocumentValidator : IValidator<AddDocumentCommand>
    {
        private readonly IEmployeeService _employeeService;
        private readonly IAttributeInfoService _attributeInfoService;
        private readonly IDocumentService _documentService;

        public AddDocumentValidator(
            IEmployeeService employeeService, 
            IAttributeInfoService attributeInfoService, 
            IDocumentService documentService)
        {
            _employeeService = employeeService;
            _attributeInfoService = attributeInfoService;
            _documentService = documentService;
        }
        
        public async Task Validate(List<ValidationFailure> list, AddDocumentCommand request)
        {
            var tasks = new List<Task>
            {
                CheckEmployee(list, request),
                CheckAttributeInfo(list, request)
            };

            foreach (var task in tasks)
            {
                await task.ConfigureAwait(false);
            }
        }

        private async Task CheckAttributeInfo(List<ValidationFailure> list, AddDocumentCommand request)
        {
            var attribute = await _attributeInfoService.GetById(request.AttributeInfoId);
            if (attribute == null)
            {
                list.Add("Attribute not found.");
            }
        }

        private Task<bool> CheckEmployee(List<ValidationFailure> list, AddDocumentCommand request)
        {
            var employee = _employeeService.GetByLogin(request.EmployeeLogin);
            if (employee == null)
            {
                list.Add("Employee not found.");
            }

            return Task.FromResult(true);
        }
    }
}