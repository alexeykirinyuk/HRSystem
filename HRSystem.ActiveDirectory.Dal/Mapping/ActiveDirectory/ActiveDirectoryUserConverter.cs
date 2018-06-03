using System.DirectoryServices.Protocols;
using AutoMapper;
using HRSystem.ActiveDirectory.Extensions;
using HRSystem.Domain;
using static HRSystem.ActiveDirectory.ActiveDirectoryConstants;

namespace HRSystem.ActiveDirectory.Dal.Mapping.ActiveDirectory
{
    internal sealed class ActiveDirectoryUserConverter : ITypeConverter<SearchResultEntry, Account>
    {
        public Account Convert(SearchResultEntry source, Account destination, ResolutionContext context)
        {
            if (destination == null)
            {
                destination = new Account();
            }

            destination.DistinguishedName = source.GetPropertyValue(EntityAttributes.DistinguishedName);
            destination.Login = source.GetPropertyValue(EntityAttributes.AccountName);
            destination.FirstName = source.GetPropertyValue(EntityAttributes.FirstName);
            destination.LastName = source.GetPropertyValue(EntityAttributes.LastName);
            destination.Email = source.GetPropertyValue(EntityAttributes.Email);
            destination.Phone = source.GetPropertyValue(EntityAttributes.Phone);
            destination.Office = source.GetPropertyValue(EntityAttributes.Office);
            destination.ManagerDistinguishedName = source.GetPropertyValue(EntityAttributes.Manager);
            destination.JobTitle = source.GetPropertyValue(EntityAttributes.Job);

            return destination;
        }
    }
}