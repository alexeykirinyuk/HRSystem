using System.DirectoryServices.Protocols;
using AutoMapper;
using HRSystem.ActiveDirectory.Extensions;
using HRSystem.Domain;

namespace HRSystem.ActiveDirectory.Dal.Mapping.ActiveDirectory
{
    internal sealed class ActiveDirectoryUserConverter : ITypeConverter<SearchResultEntry, User>
    {
        public User Convert(SearchResultEntry source, User destination, ResolutionContext context)
        {
            if (destination == null)
            {
                destination = new User();
            }

            destination.DistinguishedName = source.GetPropertyValue(ActiveDirectoryConstants.EntityAttributes.DistinguishedName);
            destination.Login = source.GetPropertyValue(ActiveDirectoryConstants.EntityAttributes.AccountName);
            destination.FirstName = source.GetPropertyValue(ActiveDirectoryConstants.EntityAttributes.FirstName);
            destination.LastName = source.GetPropertyValue(ActiveDirectoryConstants.EntityAttributes.LastName);
            destination.Email = source.GetPropertyValue(ActiveDirectoryConstants.EntityAttributes.Email);
            destination.Phone = source.GetPropertyValue(ActiveDirectoryConstants.EntityAttributes.Phone);
            destination.Office = source.GetPropertyValue(ActiveDirectoryConstants.EntityAttributes.Office);
            destination.ManagerDistinguishedName = source.GetPropertyValue(ActiveDirectoryConstants.EntityAttributes.Manager);
            destination.JobTitle = source.GetPropertyValue(ActiveDirectoryConstants.EntityAttributes.Job);

            return destination;
        }
    }
}