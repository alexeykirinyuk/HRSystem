using System.DirectoryServices.Protocols;
using AutoMapper;
using HRSystem.Domain;
using OneInc.ADEditor.ActiveDirectory.Extensions;
using OneInc.ADEditor.Models;
using static OneInc.ADEditor.ActiveDirectory.ActiveDirectoryConstants;

namespace OneInc.ADEditor.Dal.Mapping.ActiveDirectory
{
    internal sealed class ActiveDirectoryUserConverter : ITypeConverter<SearchResultEntry, Employee>
    {
        public Employee Convert(SearchResultEntry source, Employee destination, ResolutionContext context)
        {
            if (destination == null)
            {
                destination = new Employee();
            }

            destination.Login = source.GetPropertyValue(EntityAttributes.UserPrincipalName);
            destination.FirstName = source.GetPropertyValue(EntityAttributes.FirstName);
            destination.LastName = source.GetPropertyValue(EntityAttributes.LastName);
            destination.Email = source.GetPropertyValue(EntityAttributes.Email);
            destination.Phone = source.GetPropertyValue(EntityAttributes.Phone);
            destination.Office = source.GetPropertyValue(EntityAttributes.Office);

            var managerDistinguishedName = source.GetPropertyValue(EntityAttributes.Manager);
            if (!string.IsNullOrEmpty(managerDistinguishedName))
            {
                destination.Manager = new User { DistinguishedName = managerDistinguishedName };
            }

            var departmentName = source.GetPropertyValue(EntityAttributes.Department);
            if (!string.IsNullOrWhiteSpace(departmentName))
            {
                destination.Department = new Product { Name = departmentName };
            }

            var jobTitle = source.GetPropertyValue(EntityAttributes.Job);
            if (!string.IsNullOrWhiteSpace(jobTitle))
            {
                destination.Job = new Job { Title = jobTitle };
            }

            return destination;
        }
    }
}
