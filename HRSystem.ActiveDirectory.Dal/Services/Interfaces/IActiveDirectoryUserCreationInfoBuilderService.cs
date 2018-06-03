using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using HRSystem.Domain;

namespace HRSystem.ActiveDirectory.Dal.Services.Interfaces
{
    public interface IActiveDirectoryUserCreationInfoBuilderService
    {
        IEnumerable<DirectoryAttribute> BuildUserCreationInfo(User user, string password);

        string GeneratePassword();
    }
}
