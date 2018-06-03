using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using HRSystem.ActiveDirectory.Dal.Extensions;
using HRSystem.ActiveDirectory.Dal.Services.Interfaces;
using HRSystem.Domain;

namespace HRSystem.ActiveDirectory.Dal.Services
{
    public class ActiveDirectoryUserUpdatingInfoBuilderService : IActiveDirectoryUserUpdatingInfoBuilderService
    {
        public IEnumerable<DirectoryAttributeModification> BuildUserUpdatingInfo(Account updatedAccount, Account oldAccount)
        {
            var modifications = new List<DirectoryAttributeModification>();
            modifications.AddModificationIfDataChanged(
                ActiveDirectoryConstants.EntityAttributes.DisplayName,
                updatedAccount.FullName,
                oldAccount.FullName,
                u => u);
            modifications.AddModificationIfDataChanged(ActiveDirectoryConstants.EntityAttributes.FirstName, updatedAccount.FirstName,
                oldAccount.FirstName, u => u);
            modifications.AddModificationIfDataChanged(ActiveDirectoryConstants.EntityAttributes.LastName, updatedAccount.LastName,
                oldAccount.LastName, u => u);

            modifications.AddModificationIfDataChanged(ActiveDirectoryConstants.EntityAttributes.Email, updatedAccount.Email, oldAccount.Email,
                u => u);
            modifications.AddModificationIfDataChanged(ActiveDirectoryConstants.EntityAttributes.Phone, updatedAccount.Phone, oldAccount.Phone,
                u => u);

            modifications.AddModificationIfDataChanged(
                ActiveDirectoryConstants.EntityAttributes.Manager,
                updatedAccount,
                oldAccount,
                u => u.ManagerDistinguishedName);
            modifications.AddModificationIfDataChanged(
                ActiveDirectoryConstants.EntityAttributes.Office,
                updatedAccount,
                oldAccount,
                u => u.Office);
            modifications.AddModificationIfDataChanged(
                ActiveDirectoryConstants.EntityAttributes.Job,
                updatedAccount,
                oldAccount,
                u => u.JobTitle);

            return modifications;
        }
    }
}