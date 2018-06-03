using System.Threading;
using System.Threading.Tasks;
using HRSystem.Core;
using MediatR;

namespace HRSystem.Commands.AddDocument
{
    public class AddDocumentCommandHandler : IRequestHandler<AddDocumentCommand>
    {
        private readonly IEmployeeService _employeeService;
        private readonly IAttributeService _attributeService;
        private readonly IDocumentService _documentService;

        public AddDocumentCommandHandler(
            IEmployeeService employeeService, 
            IAttributeService attributeService, 
            IDocumentService documentService)
        {
            _employeeService = employeeService;
            _attributeService = attributeService;
            _documentService = documentService;
        }

        public async Task Handle(AddDocumentCommand request, CancellationToken cancellationToken)
        {
            var employee = await _employeeService.GetByLogin(request.EmployeeLogin);
            var attribute = await _attributeService.GetById(request.AttributeInfoId);

            _documentService.Save(employee, attribute, request.Document);
        }
    }
}