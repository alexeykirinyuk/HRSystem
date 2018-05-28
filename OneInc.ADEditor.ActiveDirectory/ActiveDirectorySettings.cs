using System;
using System.Collections.Generic;

namespace OneInc.ADEditor.ActiveDirectory
{
    public class ActiveDirectorySettings
    {
        public string Server { get; set; }

        public string Domain { get; set; }

        public Dictionary<string, string> Paths { get; set; }

        public string UserCreationPathPrefix { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public TimeSpan Timeout { get; set; }

        public TechincalUserAuthenticationMode TechincalUserAuthenticationMode { get; set; }

        public bool SecureSocketLayer { get; set; }

        public int ProtocolVersion { get; set; }
    }
}
