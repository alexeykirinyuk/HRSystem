using System.Collections.Generic;
using System.Linq;
using System.Text;
using OneInc.ADEditor.ActiveDirectory.Services.Requests.Interfaces;
using static OneInc.ADEditor.ActiveDirectory.ActiveDirectoryConstants;

namespace OneInc.ADEditor.ActiveDirectory.Services.Requests
{
    public class ActiveDirectoryFilterBuildingService : IActiveDirectoryFilterBuildingService
    {
        public string BuildFilterForGettingAllEntities(string entityType)
        {
            return BuildFilterForFindingEntities(entityType, string.Empty);
        }

        public string BuildFilterForGettingAllBlockedUsers()
        {
            return $"(&({EntityAttributes.Type}={Entities.User})({EntityAttributes.UserAccountControlBlocked}={EntityAttributes.UserAccountControlBlockedValue}))";
        }

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

        public string BuildFilterForCheckingExistsUserWithSameDistinguishedNameOrAccountName(
            string userDistinguishedName,
            string accountName)
        {
            var filter = $"(|({EntityAttributes.DistinguishedName}={userDistinguishedName})"
                + $"({EntityAttributes.AccountName}={accountName}))";

            return BuildFilterForFindingEntities(Entities.User, filter);
        }

        public string BuildFilterForGettingOfficeByLocation(string location)
        {
            var filter = $"({EntityAttributes.Location}={location})";

            return BuildFilterForFindingEntities(Entities.OrganizationalUnit, filter);
        }

        public string BuildFilterForGettingOfficesByLocations(IEnumerable<string> locations)
        {
            var filterBuilder = new StringBuilder("(|");
            foreach (var location in locations)
            {
                filterBuilder.Append($"({EntityAttributes.Location}={location})");
            }
            
            filterBuilder.Append(")");

            return BuildFilterForFindingEntities(Entities.OrganizationalUnit, filterBuilder.ToString());
        }

        public string BuildFilterForGettingUserByEmail(string email)
        {
            var filter = $"({EntityAttributes.Email}={email})";

            return BuildFilterForFindingEntities(Entities.User, filter);
        }

        public string BuildFilterForGettingUserByPrincipalName(string principalName)
        {
            var filter = $"({EntityAttributes.UserPrincipalName}={principalName})";

            return BuildFilterForFindingEntities(Entities.User, filter);
        }

        public string BuildFilterForGettingUsersByEmails(IEnumerable<string> emails)
        {
            var filter = new StringBuilder("(|");
            foreach (var email in emails)
            {
                filter.Append($"({EntityAttributes.Email}={email})");
            }
            filter.Append(")");

            return BuildFilterForFindingEntities(Entities.User, filter.ToString());
        }

        public string BuildFilterForGettingUsersByUserPrincipalNames(ICollection<string> userPrincipals)
        {
            var filter = new StringBuilder("(|");
            foreach (var userPrincipal in userPrincipals)
            {
                filter.Append($"({EntityAttributes.UserPrincipalName}={userPrincipal})");
            }
            filter.Append(")");

            return BuildFilterForFindingEntities(Entities.User, filter.ToString());
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
