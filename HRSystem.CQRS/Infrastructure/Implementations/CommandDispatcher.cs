using HRSystem.Common.Errors;
using HRSystem.CQRS.Infrastructure.Interfaces;

namespace HRSystem.CQRS.Infrastructure.Implementations
{
    public sealed class CommandDispatcher : ICommandDispatcher
    {
        private readonly IHandlerProvider _handlerProvider;

        public CommandDispatcher(IHandlerProvider handlerProvider)
        {
            ArgumentValidator.EnsureNotNull(nameof(handlerProvider), handlerProvider);
            
            _handlerProvider = handlerProvider;
        }

        public void Dispatch<TDefinition>(ICommandDefinition<TDefinition> commandDefinition)
            where TDefinition : ICommandDefinition<TDefinition>
        {
            ArgumentValidator.EnsureNotNull(nameof(commandDefinition), commandDefinition);
            
            var handler = _handlerProvider.GetCommandHandler<TDefinition>();
            var parameters = UnwrapDefinitionType(commandDefinition);
            handler.Handle(parameters);
        }

        private static TDefinition UnwrapDefinitionType<TDefinition>(ICommandDefinition<TDefinition> commandDefinition)
            where TDefinition : ICommandDefinition<TDefinition>
        {
            return (TDefinition)commandDefinition;
        }
    }

}