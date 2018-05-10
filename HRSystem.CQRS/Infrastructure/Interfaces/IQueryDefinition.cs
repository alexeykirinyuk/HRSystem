namespace HRSystem.CQRS.Infrastructure.Interfaces
{
    public interface IQueryDefinition<TParameters, TResult>
        where TParameters : IQueryDefinition<TParameters, TResult>
    {
    }
}