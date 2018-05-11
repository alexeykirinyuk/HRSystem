namespace HRSystem.Infrastructure.Infrastructure.Interfaces
{
    public interface ICommandHandler<in TDefinition>
        where TDefinition : ICommandDefinition<TDefinition>
    {
        void Handle(TDefinition parameters);
    }
}