using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using HRSystem.ActiveDirectory.Dal.Extensions;
using HRSystem.ActiveDirectory.Dal.Services.Interfaces;
using HRSystem.Domain;

namespace HRSystem.ActiveDirectory.Dal.Services
{
    public class ActiveDirectoryUserUpdatingInfoBuilderService : IActiveDirectoryUserUpdatingInfoBuilderService
    {
        public IEnumerable<DirectoryAttributeModification> BuildUserUpdatingInfo(User updatedUser, User oldUser)
        {
            var modifications = new List<DirectoryAttributeModification>();
            modifications.AddModificationIfDataChanged(
                ActiveDirectoryConstants.EntityAttributes.DisplayName,
                updatedUser.FullName,
                oldUser.FullName,
                u => u);
            modifications.AddModificationIfDataChanged(ActiveDirectoryConstants.EntityAttributes.FirstName, updatedUser.FirstName,
                oldUser.FirstName, u => u);
            modifications.AddModificationIfDataChanged(ActiveDirectoryConstants.EntityAttributes.LastName, updatedUser.LastName,
                oldUser.LastName, u => u);

            modifications.AddModificationIfDataChanged(ActiveDirectoryConstants.EntityAttributes.Email, updatedUser.Email, oldUser.Email,
                u => u);
            modifications.AddModificationIfDataChanged(ActiveDirectoryConstants.EntityAttributes.Phone, updatedUser.Phone, oldUser.Phone,
                u => u);

            modifications.AddModificationIfDataChanged(
                ActiveDirectoryConstants.EntityAttributes.Manager,
                updatedUser,
                oldUser,
                u => u.ManagerDistinguishedName);
            modifications.AddModificationIfDataChanged(
                ActiveDirectoryConstants.EntityAttributes.Office,
                updatedUser,
                oldUser,
                u => u.Office);
            modifications.AddModificationIfDataChanged(
                ActiveDirectoryConstants.EntityAttributes.Job,
                updatedUser,
                oldUser,
                u => u.JobTitle);

            return modifications;
        }
    }
}