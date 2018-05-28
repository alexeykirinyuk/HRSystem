using HRSystem.Domain.Attributes.Base;
using MediatR;

namespace HRSystem.Commands.SaveAttribute
{
    public class SaveAttributeCommand : IRequest
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public AttributeType Type { get; set; }
    }
}