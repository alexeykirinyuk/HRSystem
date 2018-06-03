using HRSystem.Domain;
using MediatR;

namespace HRSystem.Commands.AddDocument
{
    public class AddDocumentCommand : IRequest
    {
        public string EmployeeLogin { get; set; }
        
        public int AttributeInfoId { get; set; }
        
        public Document Document { get; set; }
    }
}