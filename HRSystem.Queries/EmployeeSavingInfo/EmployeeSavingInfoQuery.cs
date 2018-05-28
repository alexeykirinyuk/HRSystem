using MediatR;

namespace HRSystem.Queries.EmployeeSavingInfo
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class EmployeeSavingInfoQuery : IRequest<EmployeeSavingInfoQueryResponse>
    {
        public string Login { get; set; }
        
        public bool IsCreate { get; set; }
    }
}