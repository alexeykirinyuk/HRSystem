using HRSystem.Common.Errors;
using HRSystem.CQRS.Infrastructure.Interfaces;

namespace HRSystem.CQRS.Infrastructure.Implementations
{
    public abstract class QueryHandlerBase<TDefinition, TResult> : IQueryHandler<TDefinition, TResult>
        where TDefinition : IQueryDefinition<TDefinition, TResult>
    {
        public TResult Execute(TDefinition parameters)
        {
            ArgumentValidator.EnsureNotNull(nameof(parameters), parameters);
            
            CheckSecurity(parameters);

            return ExecuteImpl(parameters);
        }

        protected virtual void CheckSecurity(TDefinition parameters)
        {
        }

        protected abstract TResult ExecuteImpl(TDefinition parameters);
    }
}