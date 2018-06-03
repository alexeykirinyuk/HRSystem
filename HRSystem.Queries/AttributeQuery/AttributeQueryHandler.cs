using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HRSystem.Common.Errors;
using HRSystem.Core;
using MediatR;

namespace HRSystem.Queries.AttributeQuery
{
    public class AttributeQueryHandler : IRequestHandler<AttributeQuery, AttributeQueryResponse>
    {
        private readonly IAttributeInfoService _attributeInfoService;

        public AttributeQueryHandler(IAttributeInfoService attributeInfoService)
        {
            ArgumentHelper.EnsureNotNull(nameof(attributeInfoService), attributeInfoService);

            _attributeInfoService = attributeInfoService;
        }

        public async Task<AttributeQueryResponse> Handle(AttributeQuery request, CancellationToken cancellationToken)
        {
            var attributes = await _attributeInfoService.GetAll().ConfigureAwait(false);
            
            return new AttributeQueryResponse
            {
                Attributes = attributes.ToArray()
            };
        }
    }
}