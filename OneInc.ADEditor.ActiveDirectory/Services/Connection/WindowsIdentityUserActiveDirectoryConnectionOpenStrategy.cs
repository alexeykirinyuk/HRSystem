using System;
using System.DirectoryServices.Protocols;
using System.Security.Principal;
using LiteGuard;
using OneInc.ADEditor.ActiveDirectory.Services.Connection.Interfaces;

namespace OneInc.ADEditor.ActiveDirectory.Services.Connection
{
    public class WindowsIdentityUserActiveDirectoryConnectionOpenStrategy : IActiveDirectoryConnectionOpenStrategy
    {
        private readonly ActiveDirectorySettings _settings;

        public WindowsIdentityUserActiveDirectoryConnectionOpenStrategy(ActiveDirectorySettings settings)
        {
            Guard.AgainstNullArgument(nameof(settings), settings);

            _settings = settings;
        }

        public IActiveDirectoryConnection OpenConnection()
        {
            var connection = CreateActiveDirectoryConnection();
            using (WindowsIdentity.Impersonate(IntPtr.Zero))
            {
                connection.Open();
            }

            return connection;
        }

        private ActiveDirectoryConnectionAdapter CreateActiveDirectoryConnection()
        {
            var ldapConnection = new LdapConnection(new LdapDirectoryIdentifier(_settings.Server))
            {
                AuthType = AuthType.Negotiate,
                Timeout = _settings.Timeout,
                SessionOptions =
                {
                    SecureSocketLayer = _settings.SecureSocketLayer,
                    ProtocolVersion = _settings.ProtocolVersion,
                    VerifyServerCertificate = (connection, certificate) => true
                }
            };

            return new ActiveDirectoryConnectionAdapter(ldapConnection);
        }
    }
}
