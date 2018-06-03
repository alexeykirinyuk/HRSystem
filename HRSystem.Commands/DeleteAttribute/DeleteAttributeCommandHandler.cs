using System.Threading;
using System.Threading.Tasks;
using HRSystem.Core;
using MediatR;

namespace HRSystem.Commands.DeleteAttribute
{
    public class DeleteAttributeCommandHandler : IRequestHandler<DeleteAttributeCommand>
    {
        private readonly IAttributeService _attributeService;

        public DeleteAttributeCommandHandler(IAttributeService attributeService)
        {
            _attributeService = attributeService;
        }

        public async Task Handle(DeleteAttributeCommand request, CancellationToken cancellationToken)
        {
            var attributeInfo = await _attributeService.GetById(request.AttributeInfoId);
            await _attributeService.Delete(attributeInfo);
        }
    }
}