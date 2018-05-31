using System.Collections.Generic;

namespace OneInc.ADEditor.ActiveDirectory.Services.Requests.Interfaces
{
    public interface IActiveDirectoryFilterBuildingService
    {
        string BuildFilterForGettingAllEntities(string entityType);

        string BuildFilterForGettingAllBlockedUsers();

        string BuildFilterForGettingByDistinguishedName(string entityType, string distinguishedName);

        string BuildPathForGettingByDistinguishedName(string distinguishedName);

        string BuildFilterForCheckingExistsUserWithSameDistinguishedNameOrAccountName(string userDistinguishedName, string accountName);

        string BuildFilterForGettingOfficeByLocation(string location);

        string BuildFilterForGettingOfficesByLocations(IEnumerable<string> locations);

        string BuildFilterForGettingUserByEmail(string email);

        string BuildFilterForGettingUserByPrincipalName(string principalName);

        string BuildFilterForGettingUsersByEmails(IEnumerable<string> emails);

        string BuildFilterForGettingUsersByUserPrincipalNames(ICollection<string> userPrincipals);

        string BuildFilterForGettingUsersUpdatedFromDate(string startDate);
    }
}
