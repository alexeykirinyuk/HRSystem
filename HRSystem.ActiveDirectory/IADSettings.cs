using System;

namespace HRSystem.ActiveDirectory
{
    public interface IADSettings
    {
        string Server { get; }

        string Login { get; }

        string Password { get; }

        TimeSpan Timeout { get; }

        bool SecureSocketLayer { get; }

        int ProtocolVersion { get; }
    }
}