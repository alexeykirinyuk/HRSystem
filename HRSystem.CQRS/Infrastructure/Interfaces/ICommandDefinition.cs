namespace HRSystem.CQRS.Infrastructure.Interfaces
{
    public interface ICommandDefinition<TParameters>
        where TParameters: ICommandDefinition<TParameters>
    {
    }
}