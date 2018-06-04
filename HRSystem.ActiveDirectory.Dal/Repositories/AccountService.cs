using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using HRSystem.ActiveDirectory.Dal.Services.Interfaces;
using HRSystem.ActiveDirectory.Extensions;
using HRSystem.ActiveDirectory.Services.Requests.Interfaces;
using HRSystem.Core;
using HRSystem.Domain;
using LiteGuard;

namespace HRSystem.ActiveDirectory.Dal.Repositories
{
    public class AccountService : IAccountService
    {
        private readonly IActiveDirectoryFilterBuildingService _filterBuildingService;
        private readonly IActiveDirectoryDistinguishedNameBuilderService _distinguishedNameBuilderService;
        private readonly IActiveDirectoryService _activeDirectoryService;
        private readonly IActiveDirectoryUserCreationInfoBuilderService _creationInfoBuilderService;
        private readonly IActiveDirectoryUserUpdatingInfoBuilderService _updatingInfoBuilderService;
        private readonly string _parentDistinguishedName;

        public AccountService(
            IActiveDirectoryFilterBuildingService filterBuildingService,
            IActiveDirectoryDistinguishedNameBuilderService distinguishedNameBuilderService,
            IActiveDirectoryService activeDirectoryService,
            IActiveDirectoryUserCreationInfoBuilderService creationInfoBuilderService,
            IActiveDirectoryUserUpdatingInfoBuilderService updatingInfoBuilderService,
            ActiveDirectorySettings activeDirectorySettings)
        {
            Guard.AgainstNullArgument(nameof(filterBuildingService), filterBuildingService);
            Guard.AgainstNullArgument(nameof(distinguishedNameBuilderService), distinguishedNameBuilderService);
            Guard.AgainstNullArgument(nameof(activeDirectoryService), activeDirectoryService);
            Guard.AgainstNullArgument(nameof(creationInfoBuilderService), creationInfoBuilderService);
            Guard.AgainstNullArgument(nameof(updatingInfoBuilderService), updatingInfoBuilderService);
            Guard.AgainstNullArgument(nameof(activeDirectorySettings), activeDirectorySettings);

            _filterBuildingService = filterBuildingService;
            _distinguishedNameBuilderService = distinguishedNameBuilderService;
            _activeDirectoryService = activeDirectoryService;
            _creationInfoBuilderService = creationInfoBuilderService;
            _updatingInfoBuilderService = updatingInfoBuilderService;

            _parentDistinguishedName = activeDirectorySettings.Paths[ActiveDirectoryConstants.Entities.User];
        }

        public Account GetByLogin(string login)
        {
            var filter = _filterBuildingService.BuildFilterForGettingUserByLogin(login);
            var entity = _activeDirectoryService.Find(_parentDistinguishedName, filter);

            var user = Mapper.Map<Account>(entity);
            if (user == null)
            {
                return null;
            }
            
            if (!string.IsNullOrEmpty(user.ManagerDistinguishedName))
            {
                user.Manager = GetByDistinguishedName(user.ManagerDistinguishedName);
            }

            return user;
        }

        public void Create(Account account)
        {
            var password = _creationInfoBuilderService.GeneratePassword();
            var attributes = _creationInfoBuilderService.BuildUserCreationInfo(account, password);
            var officeDistinguishedName = GetOfficeDistinguishedNameByLocation(account.Office);
            if (string.IsNullOrEmpty(officeDistinguishedName))
            {
                throw new InvalidOperationException("Office with same location not found.");
            }
            
            _activeDirectoryService.Create(
                officeDistinguishedName,
                account.FullName,
                attributes.ToArray());
        }

        public void Update(Account account)
        {
            var oldUser = GetByLogin(account.Login);
            account.DistinguishedName = oldUser.DistinguishedName;

            var updatingInfo = _updatingInfoBuilderService.BuildUserUpdatingInfo(account, oldUser).ToArray();
            if (!updatingInfo.Any())
            {
                return;
            }

            _activeDirectoryService.Update(account.DistinguishedName, updatingInfo);
            UpdateDistinguishedName(account, oldUser);
        }

        public Account GetByDistinguishedName(string distinguishedName)
        {
            var filter =
                _filterBuildingService.BuildFilterForGettingByDistinguishedName(ActiveDirectoryConstants.Entities.User, distinguishedName);
            var path = _filterBuildingService.BuildPathForGettingByDistinguishedName(distinguishedName);
            var result = _activeDirectoryService.Find(path, filter);
            
            return Mapper.Map<Account>(result);
        }

        private string GetOfficeDistinguishedNameByLocation(string location)
        {
            var preparedLocation = PrepareLocation(location);
            var filter = _filterBuildingService.BuildFilterForGettingOfficeByLocation(preparedLocation);
            var organizationUnits = _activeDirectoryService.FindEntities(_parentDistinguishedName, filter);

            var unit = organizationUnits.FirstOrDefault(o => o.GetPropertyValue(ActiveDirectoryConstants.EntityAttributes.Location) == preparedLocation);
            return unit == null ? _parentDistinguishedName : unit.DistinguishedName;
        }

        private static string PrepareLocation(string location)
        {
            return location.Split(',').FirstOrDefault() ?? string.Empty;
        }

        private void UpdateDistinguishedName(Account account, Account oldAccount)
        {
            var newName = account.FullName;
            if (NeedUpdateDistinguishedName(account, oldAccount.FullName, newName))
            {
                account.DistinguishedName = _activeDirectoryService.UpdateDistinguishedName(
                    account.DistinguishedName,
                    GetOfficeDistinguishedNameByLocation(account.Office),
                    newName);
            }
        }

        private bool NeedUpdateDistinguishedName(Account account, string oldName, string newName)
        {
            var parentDistinguishedName =
                _distinguishedNameBuilderService.GetParentDirectoryFromDistinguishedName(account.DistinguishedName);
            var officeNotEquals = GetOfficeDistinguishedNameByLocation(account.Office) != parentDistinguishedName;
            var nameNotEquals = newName != oldName;

            return officeNotEquals || nameNotEquals;
        }
        
        public IEnumerable<Account> GetUsersUpdatedFrom(DateTime from)
        {
            var filter = _filterBuildingService.BuildFilterForGettingUsersUpdatedFromDate(
                from.ConvertToActiveDirectoryString());
            var result = _activeDirectoryService.FindEntities(_parentDistinguishedName, filter);
            return Mapper.Map<IEnumerable<Account>>(result);
        }
    }
}