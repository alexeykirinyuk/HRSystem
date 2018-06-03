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
        private readonly IAttributeInfoService _attributeInfoService;

        public DeleteDocumentCommandHandler(
            IDocumentService documentService,
            IEmployeeService employeeService,
            IAttributeInfoService attributeInfoService)
        {
            _documentService = documentService;
            _employeeService = employeeService;
            _attributeInfoService = attributeInfoService;
        }
        
        public async Task Handle(DeleteDocumentCommand request, CancellationToken cancellationToken)
        {
            var employee = await _employeeService.GetByLogin(request.EmployeeLogin).ConfigureAwait(false);
            var attribute = await _attributeInfoService.GetById(request.AttributeInfoId).ConfigureAwait(false);

            _documentService.Delete(employee, attribute);
        }
    }
}