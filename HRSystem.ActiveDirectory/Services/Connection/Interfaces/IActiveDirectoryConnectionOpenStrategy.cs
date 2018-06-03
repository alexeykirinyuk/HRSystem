namespace HRSystem.ActiveDirectory.Services.Connection.Interfaces
{
    public interface IActiveDirectoryConnectionOpenStrategy
    {
        IActiveDirectoryConnection OpenConnection();
    }
}
