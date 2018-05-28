using MediatR;

namespace HRSystem.Queries.AttributeSavingInfo
{
    public class AttributeSavingInfoQuery : IRequest<AttributeSavingInfoQueryResponse>
    {
        public int? Id { get; set; }
    }
}