using MediatR;

namespace HRSystem.Commands.DeleteDocument
{
    public class DeleteDocumentCommand : IRequest
    {
        public string EmployeeLogin { get; set; }
        
        public int AttributeInfoId { get; set; }
    }
}