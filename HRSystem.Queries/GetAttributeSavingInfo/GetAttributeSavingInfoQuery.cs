using MediatR;

namespace HRSystem.Queries.GetAttributeSavingInfo
{
    public class GetAttributeSavingInfoQuery : IRequest<GetAttributeSavingInfoQueryResponse>
    {
        public int? Id { get; set; }
    }
}