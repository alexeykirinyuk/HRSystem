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
        private readonly IAttributeService _attributeService;

        public AttributeQueryHandler(IAttributeService attributeService)
        {
            ArgumentHelper.EnsureNotNull(nameof(attributeService), attributeService);

            _attributeService = attributeService;
        }

        public async Task<AttributeQueryResponse> Handle(AttributeQuery request, CancellationToken cancellationToken)
        {
            var attributes = await _attributeService.GetAll().ConfigureAwait(false);
            
            return new AttributeQueryResponse
            {
                Attributes = attributes.ToArray()
            };
        }
    }
}