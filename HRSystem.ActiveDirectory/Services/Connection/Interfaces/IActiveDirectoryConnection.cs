using System;
using System.DirectoryServices.Protocols;

namespace HRSystem.ActiveDirectory.Services.Connection.Interfaces
{
    public interface IActiveDirectoryConnection : IDisposable
    {
        void Open();

        DirectoryResponse SendRequest(DirectoryRequest request);
    }
}
