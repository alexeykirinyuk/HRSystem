using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using HRSystem.Domain;

namespace OneInc.ADEditor.Dal.Services.Interfaces
{
    public interface IActiveDirectoryUserCreationInfoBuilderService
    {
        IEnumerable<DirectoryAttribute> BuilUserCreationInfo(User user, string password);
    }
}
