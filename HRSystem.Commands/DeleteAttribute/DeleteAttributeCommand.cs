using MediatR;

namespace HRSystem.Commands.DeleteAttribute
{
    public class DeleteAttributeCommand : IRequest
    {
        public int AttributeInfoId { get; set; }
    }
}