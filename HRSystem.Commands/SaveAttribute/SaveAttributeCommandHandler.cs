using System.Threading;
using System.Threading.Tasks;
using HRSystem.Common.Errors;
using HRSystem.Core;
using HRSystem.Domain.Attributes.Base;
using HRSystem.Global.Extensions;
using MediatR;

namespace HRSystem.Commands.SaveAttribute
{
    public class SaveAttributeCommandHandler : IRequestHandler<SaveAttribute.SaveAttributeCommand>
    {
        private readonly IAttributeService _attributeService;

        public SaveAttributeCommandHandler(IAttributeService attributeService)
        {
            ArgumentHelper.EnsureNotNull(nameof(attributeService), attributeService);

            _attributeService = attributeService;
        }

        public async Task Handle(SaveAttributeCommand request, CancellationToken cancellationToken)
        {
            if (request.Id.HasValue)
            {
                var attribute = await _attributeService.GetById(request.Id.Value);
                attribute.Name = request.Name;
                attribute.Type = request.Type;
                
                await _attributeService.Update(attribute);
            }
            else
            {
                await _attributeService.Create(new AttributeInfo
                {
                    Name = request.Name,
                    Type = request.Type
                });
            }
        }
    }
}