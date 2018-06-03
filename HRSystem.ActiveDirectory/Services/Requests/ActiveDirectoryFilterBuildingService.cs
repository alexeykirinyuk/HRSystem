using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRSystem.ActiveDirectory.Services.Requests.Interfaces;

namespace HRSystem.ActiveDirectory.Services.Requests
{
    public class ActiveDirectoryFilterBuildingService : IActiveDirectoryFilterBuildingService
    {
        public string BuildFilterForGettingAllEntities(string entityType)
        {
            return BuildFilterForFindingEntities(entityType, string.Empty);
        }

        public string BuildFilterForGettingAllBlockedUsers()
        {
            return $"(&({ActiveDirectoryConstants.EntityAttributes.Type}={ActiveDirectoryConstants.Entities.User})({ActiveDirectoryConstants.EntityAttributes.UserAccountControlBlocked}={ActiveDirectoryConstants.EntityAttributes.UserAccountControlBlockedValue}))";
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
            var filter = $"(|({ActiveDirectoryConstants.EntityAttributes.DistinguishedName}={userDistinguishedName})"
                + $"({ActiveDirectoryConstants.EntityAttributes.AccountName}={accountName}))";

            return BuildFilterForFindingEntities(ActiveDirectoryConstants.Entities.User, filter);
        }

        public string BuildFilterForGettingOfficeByLocation(string location)
        {
            var filter = $"({ActiveDirectoryConstants.EntityAttributes.Location}={location})";

            return BuildFilterForFindingEntities(ActiveDirectoryConstants.Entities.OrganizationalUnit, filter);
        }

        public string BuildFilterForGettingOfficesByLocations(IEnumerable<string> locations)
        {
            var filterBuilder = new StringBuilder("(|");
            foreach (var location in locations)
            {
                filterBuilder.Append($"({ActiveDirectoryConstants.EntityAttributes.Location}={location})");
            }
            
            filterBuilder.Append(")");

            return BuildFilterForFindingEntities(ActiveDirectoryConstants.Entities.OrganizationalUnit, filterBuilder.ToString());
        }

        public string BuildFilterForGettingUserByEmail(string email)
        {
            var filter = $"({ActiveDirectoryConstants.EntityAttributes.Email}={email})";

            return BuildFilterForFindingEntities(ActiveDirectoryConstants.Entities.User, filter);
        }

        public string BuildFilterForGettingUserByLogin(string principalName)
        {
            var filter = $"({ActiveDirectoryConstants.EntityAttributes.AccountName}={principalName})";

            return BuildFilterForFindingEntities(ActiveDirectoryConstants.Entities.User, filter);
        }

        public string BuildFilterForGettingUsersByEmails(IEnumerable<string> emails)
        {
            var filter = new StringBuilder("(|");
            foreach (var email in emails)
            {
                filter.Append($"({ActiveDirectoryConstants.EntityAttributes.Email}={email})");
            }
            filter.Append(")");

            return BuildFilterForFindingEntities(ActiveDirectoryConstants.Entities.User, filter.ToString());
        }

        public string BuildFilterForGettingUsersByUserPrincipalNames(ICollection<string> userPrincipals)
        {
            var filter = new StringBuilder("(|");
            foreach (var userPrincipal in userPrincipals)
            {
                filter.Append($"({ActiveDirectoryConstants.EntityAttributes.UserPrincipalName}={userPrincipal})");
            }
            filter.Append(")");

            return BuildFilterForFindingEntities(ActiveDirectoryConstants.Entities.User, filter.ToString());
        }

        public string BuildFilterForGettingUsersUpdatedFromDate(string startDate)
        {
            var filter = $"({ActiveDirectoryConstants.EntityAttributes.WhenUpdated}>={startDate})";

            return BuildFilterForFindingEntities(ActiveDirectoryConstants.Entities.User, filter);
        }

        private static string BuildFilterForFindingEntities(string entityType, string filter)
        {
            return $"(&({ActiveDirectoryConstants.EntityAttributes.Type}={entityType})(!{ActiveDirectoryConstants.EntityAttributes.UserAccountControlBlocked}={ActiveDirectoryConstants.EntityAttributes.UserAccountControlBlockedValue}){filter})";
        }
    }
}
