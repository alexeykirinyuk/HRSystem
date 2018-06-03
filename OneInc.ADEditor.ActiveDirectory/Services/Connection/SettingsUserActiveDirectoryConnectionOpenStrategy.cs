using System.DirectoryServices.Protocols;
using System.Net;
using LiteGuard;
using OneInc.ADEditor.ActiveDirectory.Services.Connection.Interfaces;

namespace OneInc.ADEditor.ActiveDirectory.Services.Connection
{
    public class SettingsUserActiveDirectoryConnectionOpenStrategy : IActiveDirectoryConnectionOpenStrategy
    {
        private readonly ActiveDirectorySettings _settings;

        public SettingsUserActiveDirectoryConnectionOpenStrategy(ActiveDirectorySettings settings)
        {
            Guard.AgainstNullArgument(nameof(settings), settings);

            _settings = settings;
        }

        public IActiveDirectoryConnection OpenConnection()
        {
            var connection = CreateActiveDirectoryConnection();
            connection.Open();

            return connection;
        }

        private ActiveDirectoryConnectionAdapter CreateActiveDirectoryConnection()
        {
            var ldapConnection = new LdapConnection(new LdapDirectoryIdentifier(_settings.Server))
            {
                AuthType = AuthType.Basic,
                Timeout = _settings.Timeout,
                Credential = new NetworkCredential(_settings.Login, _settings.Password),
                SessionOptions =
                {
                    SaslMethod = "GSSAPI",
                    ProtocolVersion = _settings.ProtocolVersion
                }
            };

            return new ActiveDirectoryConnectionAdapter(ldapConnection);
        }
    }
}
