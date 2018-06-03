using System.Threading;
using System.Threading.Tasks;
using HRSystem.Core;
using MediatR;

namespace HRSystem.Commands.DeleteAttribute
{
    public class DeleteAttributeCommandHandler : IRequestHandler<DeleteAttributeCommand>
    {
        private readonly IAttributeInfoService _attributeInfoService;

        public DeleteAttributeCommandHandler(IAttributeInfoService attributeInfoService)
        {
            _attributeInfoService = attributeInfoService;
        }

        public async Task Handle(DeleteAttributeCommand request, CancellationToken cancellationToken)
        {
            var attributeInfo = await _attributeInfoService.GetById(request.AttributeInfoId);
            await _attributeInfoService.Delete(attributeInfo);
        }
    }
}