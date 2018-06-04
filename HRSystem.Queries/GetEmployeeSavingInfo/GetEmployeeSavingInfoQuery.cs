using MediatR;

namespace HRSystem.Queries.GetEmployeeSavingInfo
{
    public class GetEmployeeSavingInfoQuery : IRequest<GetEmployeeSavingInfoQueryResponse>
    {
        public string Login { get; set; }
        
        public bool IsCreate { get; set; }
    }
}