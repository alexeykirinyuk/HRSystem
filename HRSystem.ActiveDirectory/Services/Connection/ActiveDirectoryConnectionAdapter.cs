using System.DirectoryServices.Protocols;
using HRSystem.ActiveDirectory.Services.Connection.Interfaces;
using LiteGuard;

namespace HRSystem.ActiveDirectory.Services.Connection
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
