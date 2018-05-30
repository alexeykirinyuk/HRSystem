using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Text;
using HRSystem.Domain;
using LiteGuard;
using OneInc.ADEditor.ActiveDirectory;
using OneInc.ADEditor.Dal.Services.Interfaces;
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
            return new List<DirectoryAttribute>
            {
                new DirectoryAttribute(EntityAttributes.Name, user.FullName),
                new DirectoryAttribute(EntityAttributes.DisplayName, user.FullName),
                new DirectoryAttribute(EntityAttributes.AccountName, user.Login),
                new DirectoryAttribute(EntityAttributes.FirstName, user.FirstName),
                new DirectoryAttribute(EntityAttributes.LastName, user.LastName),
                new DirectoryAttribute(EntityAttributes.Type, Entities.User),
                new DirectoryAttribute(EntityAttributes.Email, user.Email),
                new DirectoryAttribute(EntityAttributes.Job, user.JobTitle),
                new DirectoryAttribute(EntityAttributes.Office, user.Office),
                new DirectoryAttribute(EntityAttributes.Phone, user.Phone),
                new DirectoryAttribute(EntityAttributes.Manager, user.Manager?.DistinguishedName),
                new DirectoryAttribute(EntityAttributes.UserAccountControl, EntityAttributes.UserAccountControlValue),
                new DirectoryAttribute(EntityAttributes.UserPrincipalName, $"{user.Login}@{_settings.Domain}"),
                new DirectoryAttribute(EntityAttributes.UserMustChangePassword,
                    EntityAttributes.UserNotMustChangePasswordValue),
                new DirectoryAttribute(EntityAttributes.Password, GetFormattedPasswordAsBytes(password))
            };
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
            return Encoding.Unicode.GetBytes($"\"{password}\"");
        }
    }
}
