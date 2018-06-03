using System.Threading;
using System.Threading.Tasks;
using HRSystem.Core;
using MediatR;

namespace HRSystem.Commands.DeleteDocument
{
    public class DeleteDocumentCommandHandler : IRequestHandler<DeleteDocumentCommand>
    {
        private readonly IDocumentService _documentService;
        private readonly IEmployeeService _employeeService;
        private readonly IAttributeService _attributeService;

        public DeleteDocumentCommandHandler(
            IDocumentService documentService,
            IEmployeeService employeeService,
            IAttributeService attributeService)
        {
            _documentService = documentService;
            _employeeService = employeeService;
            _attributeService = attributeService;
        }
        
        public async Task Handle(DeleteDocumentCommand request, CancellationToken cancellationToken)
        {
            var employee = await _employeeService.GetByLogin(request.EmployeeLogin).ConfigureAwait(false);
            var attribute = await _attributeService.GetById(request.AttributeInfoId).ConfigureAwait(false);

            _documentService.Delete(employee, attribute);
        }
    }
}