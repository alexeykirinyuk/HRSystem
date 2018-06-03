using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using HRSystem.Domain;
using OneInc.ADEditor.Dal.Extensions;
using OneInc.ADEditor.Dal.Services.Interfaces;
using static OneInc.ADEditor.ActiveDirectory.ActiveDirectoryConstants;

namespace OneInc.ADEditor.Dal.Services
{
    public class ActiveDirectoryUserUpdatingInfoBuilderService : IActiveDirectoryUserUpdatingInfoBuilderService
    {
        public IEnumerable<DirectoryAttributeModification> BuildUserUpdatingInfo(User updatedUser, User oldUser)
        {
            var modifications = new List<DirectoryAttributeModification>();
            modifications.AddModificationIfDataChanged(
                EntityAttributes.DisplayName,
                updatedUser.FullName,
                oldUser.FullName,
                u => u);
            modifications.AddModificationIfDataChanged(EntityAttributes.FirstName, updatedUser.FirstName,
                oldUser.FirstName, u => u);
            modifications.AddModificationIfDataChanged(EntityAttributes.LastName, updatedUser.LastName,
                oldUser.LastName, u => u);

            modifications.AddModificationIfDataChanged(EntityAttributes.Email, updatedUser.Email, oldUser.Email,
                u => u);
            modifications.AddModificationIfDataChanged(EntityAttributes.Phone, updatedUser.Phone, oldUser.Phone,
                u => u);

            modifications.AddModificationIfDataChanged(
                EntityAttributes.Manager,
                updatedUser,
                oldUser,
                u => u.ManagerDistinguishedName);
            modifications.AddModificationIfDataChanged(
                EntityAttributes.Office,
                updatedUser,
                oldUser,
                u => u.Office);
            modifications.AddModificationIfDataChanged(
                EntityAttributes.Job,
                updatedUser,
                oldUser,
                u => u.JobTitle);

            return modifications;
        }
    }
}