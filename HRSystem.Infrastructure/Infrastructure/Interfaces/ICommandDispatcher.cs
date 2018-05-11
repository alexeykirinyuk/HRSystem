namespace HRSystem.Infrastructure.Infrastructure.Interfaces
{
    public interface ICommandDispatcher
    {
        void Dispatch<TDefinition>(ICommandDefinition<TDefinition> commandDefinition)
            where TDefinition : ICommandDefinition<TDefinition>;
    }
}