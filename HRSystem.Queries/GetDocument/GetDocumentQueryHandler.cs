using System.Threading;
using System.Threading.Tasks;
using HRSystem.Common.Validation;
using HRSystem.Core;
using MediatR;

namespace HRSystem.Queries.GetDocument
{
    public class GetDocumentQueryHandler : IRequestHandler<GetDocumentQuery, GetDocumentQueryResponse>
    {
        private readonly IEmployeeService _employeeService;
        private readonly IAttributeInfoService _attributeInfoService;
        private readonly IDocumentService _documentService;

        public GetDocumentQueryHandler(IEmployeeService employeeService, IAttributeInfoService attributeInfoService, IDocumentService documentService)
        {
            _employeeService = employeeService;
            _attributeInfoService = attributeInfoService;
            _documentService = documentService;
        }

        public async Task<GetDocumentQueryResponse> Handle(GetDocumentQuery request, CancellationToken cancellationToken)
        {
            var employee = await _employeeService.GetByLogin(request.EmployeeLogin);
            var attribute = await _attributeInfoService.GetById(request.AttributeInfoId);
            if (employee == null)
            {
                throw new ValidationException("Employee not found.");
            }

            if (attribute == null)
            {
                throw new ValidationException("Attribute not found.");
            }
            
            if (!_documentService.IsExists(employee, attribute))
            {
                throw new ValidationException("File not found.");
            }

            return new GetDocumentQueryResponse
            {
                Document = _documentService.Load(employee, attribute)
            };
        }
    }
}