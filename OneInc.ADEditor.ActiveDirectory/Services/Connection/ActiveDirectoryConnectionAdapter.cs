using System.DirectoryServices.Protocols;
using LiteGuard;
using OneInc.ADEditor.ActiveDirectory.Services.Connection.Interfaces;

namespace OneInc.ADEditor.ActiveDirectory.Services.Connection
{
    public class ActiveDirectoryConnectionAdapter : IActiveDirectoryConnection
    {
        private readonly LdapConnection _ldapConnection;

        public ActiveDirectoryConnectionAdapter(LdapConnection ldapConnection)
        {
            Guard.AgainstNullArgument(nameof(ldapConnection), ldapConnection);

            _ldapConnection = ldapConnection;
        }

        public DirectoryResponse SendRequest(DirectoryRequest request)
        {
            return _ldapConnection.SendRequest(request);
        }

        public void Open()
        {
            _ldapConnection.Bind();
        }

        public void Dispose()
        {
            _ldapConnection?.Dispose();
        }
    }
}
