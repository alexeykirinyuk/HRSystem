using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HRSystem.Common.Errors;
using HRSystem.Core;
using MediatR;

namespace HRSystem.Queries.GetAttributesQuery
{
    public class GetAttributesQueryHandler : IRequestHandler<GetAttributesQuery, GetAttributesQueryResponse>
    {
        private readonly IAttributeInfoService _attributeInfoService;

        public GetAttributesQueryHandler(IAttributeInfoService attributeInfoService)
        {
            ArgumentHelper.EnsureNotNull(nameof(attributeInfoService), attributeInfoService);

            _attributeInfoService = attributeInfoService;
        }

        public async Task<GetAttributesQueryResponse> Handle(GetAttributesQuery request, CancellationToken cancellationToken)
        {
            var attributes = await _attributeInfoService.GetAll().ConfigureAwait(false);
            
            return new GetAttributesQueryResponse
            {
                Attributes = attributes.ToArray()
            };
        }
    }
}