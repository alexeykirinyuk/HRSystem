using System.Collections.Generic;

namespace HRSystem.ActiveDirectory.Services.Requests.Interfaces
{
    public interface IActiveDirectoryFilterBuildingService
    {
        string BuildFilterForGettingByDistinguishedName(string entityType, string distinguishedName);

        string BuildPathForGettingByDistinguishedName(string distinguishedName);

        string BuildFilterForGettingOfficeByLocation(string location);

        string BuildFilterForGettingUserByLogin(string principalName);

        string BuildFilterForGettingUsersUpdatedFromDate(string startDate);
    }
}
