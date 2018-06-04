using System.Threading;
using System.Threading.Tasks;
using HRSystem.Common.Errors;
using HRSystem.Core;
using HRSystem.Domain.Attributes.Base;
using MediatR;

namespace HRSystem.Queries.GetAttributeSavingInfo
{
    public class GetAttributeSavingInfoQueryHandler : IRequestHandler<GetAttributeSavingInfoQuery, GetAttributeSavingInfoQueryResponse>
    {
        private readonly IAttributeInfoService _attributeInfoService;

        public GetAttributeSavingInfoQueryHandler(IAttributeInfoService attributeInfoService)
        {
            ArgumentHelper.EnsureNotNull(nameof(attributeInfoService), attributeInfoService);
            
            _attributeInfoService = attributeInfoService;
        }

        public async Task<GetAttributeSavingInfoQueryResponse> Handle(GetAttributeSavingInfoQuery request, CancellationToken cancellationToken)
        {
            if (!request.Id.HasValue)
                return new GetAttributeSavingInfoQueryResponse
                {
                    Name = string.Empty,
                    Type = AttributeType.String
                };
            
            var attribute = await _attributeInfoService.GetById(request.Id.Value).ConfigureAwait(false);
            return new GetAttributeSavingInfoQueryResponse
            {
                Name = attribute.Name,
                Type = attribute.Type
            };
        }
    }
}