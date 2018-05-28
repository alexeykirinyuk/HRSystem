using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using LiteGuard;
using OneInc.ADEditor.ActiveDirectory;
using OneInc.ADEditor.Common.Utils;
using OneInc.ADEditor.Dal.Services.Interfaces;
using OneInc.ADEditor.Models;
using OneInc.ADEditor.Models.Extensions;
using static OneInc.ADEditor.ActiveDirectory.ActiveDirectoryConstants;

namespace OneInc.ADEditor.Dal.Services
{
    public class ActiveDirectoryUserCreationInfoBuilderService : IActiveDirectoryUserCreationInfoBuilderService
    {
        private readonly ActiveDirectorySettings _settings;

        public ActiveDirectoryUserCreationInfoBuilderService(ActiveDirectorySettings settings)
        {
            Guard.AgainstNullArgument(nameof(settings), settings);

            _settings = settings;
        }

        public IEnumerable<DirectoryAttribute> BuilUserCreationInfo(User user, string password)
        {
            var result = new List<DirectoryAttribute>
            {
                new DirectoryAttribute(EntityAttributes.Name, user.GetFullName()),
                new DirectoryAttribute(EntityAttributes.DisplayName, user.GetFullName()),
                new DirectoryAttribute(EntityAttributes.AccountName, user.AccountName),
                new DirectoryAttribute(EntityAttributes.FirstName, user.FirstName),
                new DirectoryAttribute(EntityAttributes.LastName, user.LastName),
                new DirectoryAttribute(EntityAttributes.Type, Entities.User),
                new DirectoryAttribute(EntityAttributes.Email, user.Email),
                new DirectoryAttribute(EntityAttributes.UserAccountControl, EntityAttributes.UserAccountControlValue)
            };

            AddAttributeIfNotNullOrEmpty(result, EntityAttributes.Phone, user.Phone, phone => phone);

            AddAttributeIfNotNullOrEmpty(result, EntityAttributes.Job, user.Job, job => job.Title);
            AddAttributeIfNotNullOrEmpty(result, EntityAttributes.Manager, user.Manager, manager => manager.DistinguishedName);
            AddAttributeIfNotNullOrEmpty(result, EntityAttributes.Office, user.Office, office => office.Location);
            AddAttributeIfNotNullOrEmpty(result, EntityAttributes.Department, user.Department, product => product.Name);

            result.Add(new DirectoryAttribute(EntityAttributes.UserPrincipalName, $"{user.AccountName}@{_settings.Domain}"));
            result.Add(new DirectoryAttribute(EntityAttributes.UserMustChangePassword, EntityAttributes.UserNotMustChangePasswordValue));
            result.Add(new DirectoryAttribute(EntityAttributes.Password, GetFormattedPasswordAsBytes(password)));

            return result.ToArray();
        }

        private static void AddAttributeIfNotNullOrEmpty<T>(
            ICollection<DirectoryAttribute> list,
            string attibuteName,
            T source,
            Func<T, string> getValueFunc)
        {
            if (source == null)
            {
                return;
            }

            var value = getValueFunc(source);

            if (!string.IsNullOrEmpty(value))
            {
                list.Add(new DirectoryAttribute(attibuteName, value));
            }
        }

        private static byte[] GetFormattedPasswordAsBytes(string password)
        {
            var formattedPassword = $"\"{password}\"";
            return formattedPassword.GetBytes();
        }
    }
}
