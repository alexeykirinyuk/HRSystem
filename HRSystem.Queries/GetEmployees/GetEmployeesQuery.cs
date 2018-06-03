using MediatR;

namespace HRSystem.Queries.GetEmployees
{
    public class GetEmployeesQuery : IRequest<GetEmployeesQueryResponse>
    {
        public string SearchFilter { get; set; }
    }
}