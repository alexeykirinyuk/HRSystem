namespace HRSystem.CQRS.Infrastructure.Interfaces
{
    public interface IHandlerProvider
    {
        IQueryHandler<TDefinition, TResult> GetQueryHandler<TDefinition, TResult>() 
            where TDefinition : IQueryDefinition<TDefinition, TResult>;

        IAsyncQueryHandler<TDefinition, TResult> GetAsyncQueryHandler<TDefinition, TResult>() 
            where TDefinition : IQueryDefinition<TDefinition, TResult>;

        ICommandHandler<TDefinition> GetCommandHandler<TDefinition>()
            where TDefinition : ICommandDefinition<TDefinition>; 
    }

}