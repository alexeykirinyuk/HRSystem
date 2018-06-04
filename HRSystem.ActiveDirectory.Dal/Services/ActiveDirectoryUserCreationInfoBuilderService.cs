using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Text;
using HRSystem.ActiveDirectory.Dal.Services.Interfaces;
using HRSystem.Domain;
using LiteGuard;
using static HRSystem.ActiveDirectory.ActiveDirectoryConstants;

namespace HRSystem.ActiveDirectory.Dal.Services
{
    public class ActiveDirectoryUserCreationInfoBuilderService : IActiveDirectoryUserCreationInfoBuilderService
    {
        private readonly ActiveDirectorySettings _settings;
        private const string Symbols = "abcdefghijklmnopqrstuvwxyz";
        private const int PasswordLength = 21;
        private static readonly Random Random = new Random();

        public ActiveDirectoryUserCreationInfoBuilderService(ActiveDirectorySettings settings)
        {
            Guard.AgainstNullArgument(nameof(settings), settings);

            _settings = settings;
        }

        public IEnumerable<DirectoryAttribute> BuildUserCreationInfo(Account account, string password)
        {
            var list = new List<DirectoryAttribute>
            {
                new DirectoryAttribute(EntityAttributes.Name, account.FullName),
                new DirectoryAttribute(EntityAttributes.DisplayName, account.FullName),
                new DirectoryAttribute(EntityAttributes.AccountName, account.Login),
                new DirectoryAttribute(EntityAttributes.FirstName, account.FirstName),
                new DirectoryAttribute(EntityAttributes.LastName, account.LastName),
                new DirectoryAttribute(EntityAttributes.Type, Entities.User),
                new DirectoryAttribute(EntityAttributes.Email, account.Email),
                new DirectoryAttribute(EntityAttributes.Job, account.JobTitle),
                new DirectoryAttribute(EntityAttributes.Office, account.Office),
                new DirectoryAttribute(EntityAttributes.Phone, account.Phone),
                new DirectoryAttribute(EntityAttributes.UserAccountControl, EntityAttributes.UserAccountControlValue),
                new DirectoryAttribute(EntityAttributes.UserPrincipalName, $"{account.Login}@{_settings.Domain}"),
                new DirectoryAttribute(EntityAttributes.UserMustChangePassword,
                    EntityAttributes.UserNotMustChangePasswordValue),
            };

            if (!string.IsNullOrEmpty(account.ManagerDistinguishedName))
            {
                list.Add(new DirectoryAttribute(EntityAttributes.Manager, account.ManagerDistinguishedName));
            }

            return list;
        }

        private static byte[] GetFormattedPasswordAsBytes(string password)
        {
            return Encoding.Unicode.GetBytes($"\"{password}\"");
        }

        public string GeneratePassword()
        {
            var passwordArray = new char[PasswordLength];
            var partLenght = passwordArray.Length / 3;

            GeneratePart(passwordArray, partLenght, 1, GetRandomSymbol);
            GeneratePart(passwordArray, partLenght, 2, () => char.ToUpper(GetRandomSymbol()));
            GeneratePart(passwordArray, partLenght, 3, () => (char) ('0' + Random.Next(10)));

            return new string(passwordArray);
        }

        private static void GeneratePart(IList<char> passwordArray, int partLenght, int partNumber,
            Func<char> getSymbolFunc)
        {
            for (var i = partLenght * (partNumber - 1); i < partLenght * partNumber; i++)
            {
                passwordArray[i] = getSymbolFunc();
            }
        }

        private static char GetRandomSymbol()
        {
            var index = Random.Next(Symbols.Length - 1);

            return Symbols[index];
        }
    }
}