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
        private readonly IAttributeInfoService _attributeInfoService;

        public SaveAttributeCommandHandler(IAttributeInfoService attributeInfoService)
        {
            ArgumentHelper.EnsureNotNull(nameof(attributeInfoService), attributeInfoService);

            _attributeInfoService = attributeInfoService;
        }

        public async Task Handle(SaveAttributeCommand request, CancellationToken cancellationToken)
        {
            if (request.Id.HasValue)
            {
                var attribute = await _attributeInfoService.GetById(request.Id.Value);
                attribute.Name = request.Name;
                attribute.Type = request.Type;
                
                await _attributeInfoService.Update(attribute);
            }
            else
            {
                await _attributeInfoService.Create(new AttributeInfo
                {
                    Name = request.Name,
                    Type = request.Type
                });
            }
        }
    }
}