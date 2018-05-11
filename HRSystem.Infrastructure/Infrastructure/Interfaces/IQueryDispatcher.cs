using System.Threading.Tasks;

namespace HRSystem.Infrastructure.Infrastructure.Interfaces
{
    public interface IQueryDispatcher
    {
        TResult Execute<TDefinition, TResult>(IQueryDefinition<TDefinition, TResult> queryDefinition)
            where TDefinition : IQueryDefinition<TDefinition, TResult>;

        Task<TResult> ExecuteAsync<TDefinition, TResult>(IQueryDefinition<TDefinition, TResult> queryDefinition)
            where TDefinition : IQueryDefinition<TDefinition, TResult>;
    }
}