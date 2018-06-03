using MediatR;

namespace HRSystem.Queries.GetDocument
{
    public class GetDocumentQuery : IRequest<GetDocumentQueryResponse>
    {
        public string EmployeeLogin { get; set; }
        
        public int AttributeInfoId { get; set; }
    }
}