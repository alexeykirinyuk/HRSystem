using System.Threading;
using System.Threading.Tasks;
using HRSystem.Common.Errors;
using HRSystem.Core;
using HRSystem.Domain.Attributes.Base;
using MediatR;

namespace HRSystem.Queries.AttributeSavingInfo
{
    public class AttributeSavingInfoQueryHandler : IRequestHandler<AttributeSavingInfoQuery, AttributeSavingInfoQueryResponse>
    {
        private readonly IAttributeService _attributeService;

        public AttributeSavingInfoQueryHandler(IAttributeService attributeService)
        {
            ArgumentHelper.EnsureNotNull(nameof(attributeService), attributeService);
            
            _attributeService = attributeService;
        }

        public async Task<AttributeSavingInfoQueryResponse> Handle(AttributeSavingInfoQuery request, CancellationToken cancellationToken)
        {
            if (!request.Id.HasValue)
                return new AttributeSavingInfoQueryResponse
                {
                    Name = string.Empty,
                    Type = AttributeType.String
                };
            
            var attribute = await _attributeService.GetById(request.Id.Value).ConfigureAwait(false);
            return new AttributeSavingInfoQueryResponse
            {
                Name = attribute.Name,
                Type = attribute.Type
            };
        }
    }
}