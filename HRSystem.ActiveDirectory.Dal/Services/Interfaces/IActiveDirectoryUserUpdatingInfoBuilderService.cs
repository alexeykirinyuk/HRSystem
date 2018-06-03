using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using HRSystem.Domain;

namespace HRSystem.ActiveDirectory.Dal.Services.Interfaces
{
    public interface IActiveDirectoryUserUpdatingInfoBuilderService
    {
        IEnumerable<DirectoryAttributeModification> BuildUserUpdatingInfo(User updatedUser, User oldUser);
    }
}
