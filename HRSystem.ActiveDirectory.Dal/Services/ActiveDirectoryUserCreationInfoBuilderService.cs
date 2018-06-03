using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Text;
using HRSystem.ActiveDirectory.Dal.Services.Interfaces;
using HRSystem.Domain;
using LiteGuard;

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
                new DirectoryAttribute(ActiveDirectoryConstants.EntityAttributes.Name, account.FullName),
                new DirectoryAttribute(ActiveDirectoryConstants.EntityAttributes.DisplayName, account.FullName),
                new DirectoryAttribute(ActiveDirectoryConstants.EntityAttributes.AccountName, account.Login),
                new DirectoryAttribute(ActiveDirectoryConstants.EntityAttributes.FirstName, account.FirstName),
                new DirectoryAttribute(ActiveDirectoryConstants.EntityAttributes.LastName, account.LastName),
                new DirectoryAttribute(ActiveDirectoryConstants.EntityAttributes.Type, ActiveDirectoryConstants.Entities.User),
                new DirectoryAttribute(ActiveDirectoryConstants.EntityAttributes.Email, account.Email),
                new DirectoryAttribute(ActiveDirectoryConstants.EntityAttributes.Job, account.JobTitle),
                new DirectoryAttribute(ActiveDirectoryConstants.EntityAttributes.Office, account.Office),
                new DirectoryAttribute(ActiveDirectoryConstants.EntityAttributes.Phone, account.Phone),
                new DirectoryAttribute(ActiveDirectoryConstants.EntityAttributes.UserAccountControl, ActiveDirectoryConstants.EntityAttributes.UserAccountControlValue),
                new DirectoryAttribute(ActiveDirectoryConstants.EntityAttributes.UserPrincipalName, $"{account.Login}@{_settings.Domain}"),
                new DirectoryAttribute(ActiveDirectoryConstants.EntityAttributes.UserMustChangePassword,
                    ActiveDirectoryConstants.EntityAttributes.UserNotMustChangePasswordValue),
//                new DirectoryAttribute(EntityAttributes.Password, GetFormattedPasswordAsBytes(password))
            };

            if (!string.IsNullOrEmpty(account.ManagerDistinguishedName))
            {
                list.Add(new DirectoryAttribute(ActiveDirectoryConstants.EntityAttributes.Manager, account.ManagerDistinguishedName));
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