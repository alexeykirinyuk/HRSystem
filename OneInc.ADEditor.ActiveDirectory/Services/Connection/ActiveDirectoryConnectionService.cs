using System;
using LiteGuard;
using OneInc.ADEditor.ActiveDirectory.Services.Connection.Interfaces;

namespace OneInc.ADEditor.ActiveDirectory.Services.Connection
{
    public class ActiveDirectoryConnectionService : IActiveDirectoryConnectionService
    {
        private readonly Func<TechincalUserAuthenticationMode, IActiveDirectoryConnectionOpenStrategy> _connectionFactory;
        private readonly ActiveDirectorySettings _settings;

        public ActiveDirectoryConnectionService(
            Func<TechincalUserAuthenticationMode, IActiveDirectoryConnectionOpenStrategy> connectionFactory,
            ActiveDirectorySettings settings)
        {
            Guard.AgainstNullArgument(nameof(connectionFactory), connectionFactory);
            Guard.AgainstNullArgument(nameof(settings), settings);

            _connectionFactory = connectionFactory;
            _settings = settings;
        }

        public IActiveDirectoryConnection OpenConnection()
        {
            return _connectionFactory(_settings.TechincalUserAuthenticationMode).OpenConnection();
        }
    }
}
