using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using HRSystem.Domain;

namespace OneInc.ADEditor.Dal.Services.Interfaces
{
    public interface IActiveDirectoryUserUpdatingInfoBuilderService
    {
        IEnumerable<DirectoryAttributeModification> BuildUserUpdatingInfo(User updatedUser, User oldUser);
    }
}
