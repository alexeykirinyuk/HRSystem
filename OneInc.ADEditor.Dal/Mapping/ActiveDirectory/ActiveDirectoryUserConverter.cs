using System.DirectoryServices.Protocols;
using AutoMapper;
using HRSystem.Domain;
using OneInc.ADEditor.ActiveDirectory.Extensions;
using static OneInc.ADEditor.ActiveDirectory.ActiveDirectoryConstants;

namespace OneInc.ADEditor.Dal.Mapping.ActiveDirectory
{
    internal sealed class ActiveDirectoryUserConverter : ITypeConverter<SearchResultEntry, User>
    {
        public User Convert(SearchResultEntry source, User destination, ResolutionContext context)
        {
            if (destination == null)
            {
                destination = new User();
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