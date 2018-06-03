using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRSystem.ActiveDirectory.Services.Requests.Interfaces;
using static HRSystem.ActiveDirectory.ActiveDirectoryConstants;

namespace HRSystem.ActiveDirectory.Services.Requests
{
    public class ActiveDirectoryFilterBuildingService : IActiveDirectoryFilterBuildingService
    {
        public string BuildFilterForGettingByDistinguishedName(string entityType, string distinguishedName)
        {
            var splitted = distinguishedName.Split(',');
            var filter = $"(({splitted[0]}))";

            return BuildFilterForFindingEntities(entityType, filter);
        }

        public string BuildPathForGettingByDistinguishedName(string distinguishedName)
        {
            var splitted = distinguishedName.Split(',');

            return string.Join(",", splitted.Skip(1));
        }

        public string BuildFilterForGettingOfficeByLocation(string location)
        {
            var filter = $"({EntityAttributes.Location}={location})";

            return BuildFilterForFindingEntities(Entities.OrganizationalUnit, filter);
        }

        public string BuildFilterForGettingUserByLogin(string principalName)
        {
            var filter = $"({EntityAttributes.AccountName}={principalName})";

            return BuildFilterForFindingEntities(Entities.User, filter);
        }

        public string BuildFilterForGettingUsersUpdatedFromDate(string startDate)
        {
            var filter = $"({EntityAttributes.WhenUpdated}>={startDate})";

            return BuildFilterForFindingEntities(Entities.User, filter);
        }

        private static string BuildFilterForFindingEntities(string entityType, string filter)
        {
            return $"(&({EntityAttributes.Type}={entityType})(!{EntityAttributes.UserAccountControlBlocked}={EntityAttributes.UserAccountControlBlockedValue}){filter})";
        }
    }
}
