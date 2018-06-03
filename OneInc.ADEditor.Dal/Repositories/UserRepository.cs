using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using HRSystem.Core;
using HRSystem.Domain;
using LiteGuard;
using OneInc.ADEditor.ActiveDirectory;
using OneInc.ADEditor.ActiveDirectory.Extensions;
using OneInc.ADEditor.ActiveDirectory.Services.Requests.Interfaces;
using OneInc.ADEditor.Dal.Services.Interfaces;
using static OneInc.ADEditor.ActiveDirectory.ActiveDirectoryConstants;

namespace OneInc.ADEditor.Dal.Repositories
{
    public class UserService : IUserService
    {
        private readonly IActiveDirectoryFilterBuildingService _filterBuildingService;
        private readonly IActiveDirectoryDistinguishedNameBuilderService _distinguishedNameBuilderService;
        private readonly IActiveDirectoryService _activeDirectoryService;
        private readonly IActiveDirectoryUserCreationInfoBuilderService _creationInfoBuilderService;
        private readonly IActiveDirectoryUserUpdatingInfoBuilderService _updatingInfoBuilderService;
        private readonly string _parentDistinguishedName;

        public UserService(
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

            _parentDistinguishedName = activeDirectorySettings.Paths[Entities.User];
        }

        public User GetByLogin(string login)
        {
            var filter = _filterBuildingService.BuildFilterForGettingUserByLogin(login);
            var entity = _activeDirectoryService.Find(_parentDistinguishedName, filter);

            var user = Mapper.Map<User>(entity);
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

        public void Create(User user)
        {
            var password = _creationInfoBuilderService.GeneratePassword();
            var attributes = _creationInfoBuilderService.BuildUserCreationInfo(user, password);
            _activeDirectoryService.Create(
                GetOfficeDistinguishedNameByLocation(user.Office),
                user.FullName,
                attributes.ToArray());
        }

        public void Update(User user)
        {
            var oldUser = GetByLogin(user.Login);
            user.DistinguishedName = oldUser.DistinguishedName;

            var updatingInfo = _updatingInfoBuilderService.BuildUserUpdatingInfo(user, oldUser).ToArray();
            if (!updatingInfo.Any())
            {
                return;
            }

            _activeDirectoryService.Update(user.DistinguishedName, updatingInfo);
            UpdateDistinguishedName(user, oldUser);
        }

        public User GetByDistinguishedName(string distinguishedName)
        {
            var filter =
                _filterBuildingService.BuildFilterForGettingByDistinguishedName(Entities.User, distinguishedName);
            var path = _filterBuildingService.BuildPathForGettingByDistinguishedName(distinguishedName);
            var result = _activeDirectoryService.Find(path, filter);
            
            return Mapper.Map<User>(result);
        }

        private string GetOfficeDistinguishedNameByLocation(string location)
        {
            var preparedLocation = PrepareLocation(location);
            var filter = _filterBuildingService.BuildFilterForGettingOfficeByLocation(preparedLocation);
            var organizationUnits = _activeDirectoryService.FindEntities(_parentDistinguishedName, filter);

            var unit = organizationUnits.FirstOrDefault(o => o.GetPropertyValue(EntityAttributes.Location) == preparedLocation);
            return unit == null ? _parentDistinguishedName : unit.DistinguishedName;
        }

        private static string PrepareLocation(string location)
        {
            return location.Split(',').FirstOrDefault() ?? string.Empty;
        }

        private void UpdateDistinguishedName(User user, User oldUser)
        {
            var newName = user.FullName;
            if (NeedUpdateDistinguishedName(user, oldUser.FullName, newName))
            {
                user.DistinguishedName = _activeDirectoryService.UpdateDistinguishedName(
                    user.DistinguishedName,
                    GetOfficeDistinguishedNameByLocation(user.Office),
                    newName);
            }
        }

        private bool NeedUpdateDistinguishedName(User user, string oldName, string newName)
        {
            var parentDistinguishedName =
                _distinguishedNameBuilderService.GetParentDirectoryFromDistinguishedName(user.DistinguishedName);
            var officeNotEquals = GetOfficeDistinguishedNameByLocation(user.Office) != parentDistinguishedName;
            var nameNotEquals = newName != oldName;

            return officeNotEquals || nameNotEquals;
        }
        
        public IEnumerable<User> GetUsersUpdatedFrom(DateTime from)
        {
            var filter = _filterBuildingService.BuildFilterForGettingUsersUpdatedFromDate(
                from.ConvertToActiveDirectoryString());
            var result = _activeDirectoryService.FindEntities(_parentDistinguishedName, filter);
            return Mapper.Map<IEnumerable<User>>(result);
        }
    }
}