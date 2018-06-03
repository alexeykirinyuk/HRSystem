using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HRSystem.Common.Validation;
using HRSystem.Core;
using HRSystem.Global.Validation;

namespace HRSystem.Commands.DeleteDocument
{
    public class DeleteDocumentValidator : IValidator<DeleteDocumentCommand>
    {
        private readonly IDocumentService _documentService;
        private readonly IEmployeeService _employeeService;
        private readonly IAttributeInfoService _attributeInfoService;

        public DeleteDocumentValidator(
            IDocumentService documentService,
            IEmployeeService employeeService,
            IAttributeInfoService attributeInfoService)
        {
            _documentService = documentService;
            _employeeService = employeeService;
            _attributeInfoService = attributeInfoService;
        }

        public async Task Validate(List<ValidationFailure> list, DeleteDocumentCommand request)
        {
            if (!await _employeeService.IsExists(request.EmployeeLogin))
            {
                list.Add("Employee not found.");
                return;
            }

            if (!await _attributeInfoService.IsExists(request.AttributeInfoId))
            {
                list.Add("Attribute not found.");
            }

            var employee = await _employeeService.GetByLogin(request.EmployeeLogin);
            var attribute = await _attributeInfoService.GetById(request.AttributeInfoId);
            if (!_documentService.IsExists(employee, attribute))
            {
                list.Add("Document not found.");
            }
        }
    }
}