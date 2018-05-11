using HRSystem.Global.Errors;
using HRSystem.Infrastructure.Infrastructure.Interfaces;

namespace HRSystem.Infrastructure.Infrastructure.Implementations
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