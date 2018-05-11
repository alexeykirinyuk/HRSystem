using System.Threading.Tasks;

namespace HRSystem.Infrastructure.Infrastructure.Interfaces
{
    public interface IAsyncQueryHandler<in TDefinition, TResult>
        where TDefinition : IQueryDefinition<TDefinition, TResult>
    {
        Task<TResult> ExecuteAsync(TDefinition parameters);
    }

}