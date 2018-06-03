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
        private readonly IAttributeInfoService _attributeInfoService;

        public AttributeSavingInfoQueryHandler(IAttributeInfoService attributeInfoService)
        {
            ArgumentHelper.EnsureNotNull(nameof(attributeInfoService), attributeInfoService);
            
            _attributeInfoService = attributeInfoService;
        }

        public async Task<AttributeSavingInfoQueryResponse> Handle(AttributeSavingInfoQuery request, CancellationToken cancellationToken)
        {
            if (!request.Id.HasValue)
                return new AttributeSavingInfoQueryResponse
                {
                    Name = string.Empty,
                    Type = AttributeType.String
                };
            
            var attribute = await _attributeInfoService.GetById(request.Id.Value).ConfigureAwait(false);
            return new AttributeSavingInfoQueryResponse
            {
                Name = attribute.Name,
                Type = attribute.Type
            };
        }
    }
}