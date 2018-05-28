using System;
using System.DirectoryServices.Protocols;

namespace OneInc.ADEditor.ActiveDirectory.Services.Connection.Interfaces
{
    public interface IActiveDirectoryConnection : IDisposable
    {
        void Open();

        DirectoryResponse SendRequest(DirectoryRequest request);
    }
}
