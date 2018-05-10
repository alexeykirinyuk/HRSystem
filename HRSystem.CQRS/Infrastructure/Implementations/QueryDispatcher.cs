using System.Threading.Tasks;
using HRSystem.Common.Errors;
using HRSystem.CQRS.Infrastructure.Interfaces;

namespace HRSystem.CQRS.Infrastructure.Implementations
{
    public sealed class QueryDispatcher : IQueryDispatcher
    {
        private readonly IHandlerProvider _handlerProvider;

        public QueryDispatcher(IHandlerProvider handlerProvider)
        {
            ArgumentValidator.EnsureNotNull(nameof(handlerProvider), handlerProvider);
            
            _handlerProvider = handlerProvider;
        }

        public TResult Execute<TDefinition, TResult>(IQueryDefinition<TDefinition, TResult> queryDefinition) 
            where TDefinition : IQueryDefinition<TDefinition, TResult>
        {
            ArgumentValidator.EnsureNotNull(nameof(queryDefinition), queryDefinition);
            
            var handler = _handlerProvider.GetQueryHandler<TDefinition, TResult>();
            var parameters = UnwrapDefinitionType(queryDefinition);
            
            return handler.Execute(parameters);
        }

        public Task<TResult> ExecuteAsync<TDefinition, TResult>(IQueryDefinition<TDefinition, TResult> queryDefinition) 
            where TDefinition : IQueryDefinition<TDefinition, TResult>
        {
            ArgumentValidator.EnsureNotNull(nameof(queryDefinition), queryDefinition);
            
            var handler = _handlerProvider.GetAsyncQueryHandler<TDefinition, TResult>();
            var parameters = UnwrapDefinitionType(queryDefinition);
            
            return handler.ExecuteAsync(parameters);
        }

        private static TDefinition UnwrapDefinitionType<TDefinition, TResult>(IQueryDefinition<TDefinition, TResult> queryDefinition)
            where TDefinition : IQueryDefinition<TDefinition, TResult>
        {
            return (TDefinition)queryDefinition;
        }
    }

}