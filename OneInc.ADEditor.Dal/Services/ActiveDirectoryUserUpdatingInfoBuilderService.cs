using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using OneInc.ADEditor.Dal.Extensions;
using OneInc.ADEditor.Dal.Services.Interfaces;
using OneInc.ADEditor.Models;
using OneInc.ADEditor.Models.Extensions;
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
                updatedUser.GetFullName(),
                oldUser.GetFullName(),
                u => u);
            modifications.AddModificationIfDataChanged(EntityAttributes.FirstName, updatedUser.FirstName, oldUser.FirstName, u => u);
            modifications.AddModificationIfDataChanged(EntityAttributes.LastName, updatedUser.LastName, oldUser.LastName, u => u);
            // TODO: There is not possibility to update mail address as crucible may stop worked.
//            modifications.AddModificationIfDataChanged(EntityAttributes.Email, updatedUser.Email, oldUser.Email, u => u);
            modifications.AddModificationIfDataChanged(EntityAttributes.Phone, updatedUser.Phone, oldUser.Phone, u => u);

            modifications.AddModificationIfDataChanged(
                EntityAttributes.Manager,
                updatedUser.Manager,
                oldUser.Manager,
                u => u.DistinguishedName);
            modifications.AddModificationIfDataChanged(
                EntityAttributes.Office,
                updatedUser.Office,
                oldUser.Office,
                office => office.Location);
            modifications.AddModificationIfDataChanged(
                EntityAttributes.Department,
                updatedUser.Department,
                oldUser.Department,
                product => product.Name);
            modifications.AddModificationIfDataChanged(
                EntityAttributes.Job,
                updatedUser.Job,
                oldUser.Job,
                job => job.Title);

            return modifications;
        }
    }
}
