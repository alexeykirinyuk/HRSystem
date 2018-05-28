using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using OneInc.ADEditor.Models;

namespace OneInc.ADEditor.Dal.Services.Interfaces
{
    public interface IActiveDirectoryUserCreationInfoBuilderService
    {
        IEnumerable<DirectoryAttribute> BuilUserCreationInfo(User user, string password);
    }
}
