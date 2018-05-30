using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using HRSystem.Domain;
using LiteGuard;
using OneInc.ADEditor.ActiveDirectory;
using OneInc.ADEditor.ActiveDirectory.Extensions;
using OneInc.ADEditor.ActiveDirectory.Services.Requests.Interfaces;
using OneInc.ADEditor.Dal.Services.Interfaces;
using static OneInc.ADEditor.ActiveDirectory.ActiveDirectoryConstants;

namespace OneInc.ADEditor.Dal.Repositories
{
    public class UserRepository
    {
        private const string CanUpdateDistingUishedNameKey = "CanUpdate";

        private readonly IActiveDirectoryFilterBuildingService _filterBuildingService;
        private readonly IActiveDirectoryDistinguishedNameBuilderService _distinguishedNameBuilderService;
        private readonly IActiveDirectoryService _activeDirectoryService;
        private readonly IActiveDirectoryUserCreationInfoBuilderService _creationInfoBuilderService;
        private readonly IActiveDirectoryUserUpdatingInfoBuilderService _updatingInfoBuilderService;
        private readonly string _parentDistinguishedName;

        private static readonly object LockObject = new object();
        private readonly string _canUpdateDistinguishedName;

        public UserRepository(
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
            _canUpdateDistinguishedName = activeDirectorySettings.Paths[CanUpdateDistingUishedNameKey];
        }

        private User GetByLogin(string principalName)
        {
            var filter = _filterBuildingService.BuildFilterForGettingUserByPrincipalName(principalName);
            var entity = _activeDirectoryService.Find(_parentDistinguishedName, filter);

            var user = Mapper.Map<User>(entity);
            if (!string.IsNullOrEmpty(user.ManagerDistinguishedName))
            {
                user.Manager = GetByDistinguishedName(user.ManagerDistinguishedName);
            }

            return user;
        }

        public User GetByDistinguishedName(string distinguishedName)
        {
            var filter =
                _filterBuildingService.BuildFilterForGettingByDistinguishedName(Entities.User, distinguishedName);
            var path = _filterBuildingService.BuildPathForGettingByDistinguishedName(distinguishedName);
            var result = _activeDirectoryService.Find(path, filter);
            
            return Mapper.Map<User>(result);
        }

        public string Create(User user, string password)
        {
            var attributes = _creationInfoBuilderService.BuilUserCreationInfo(user, password);
            var result = _activeDirectoryService.Create(
                GetOfficeDistinguishedNameByLocation(user.Office),
                user.FullName,
                attributes.ToArray());

            return result;
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

        public bool UpdateIfNeeded(User user)
        {
            var oldUser = GetByLogin(user.Login);
            user.DistinguishedName = oldUser.DistinguishedName;

            var updatingInfo = _updatingInfoBuilderService.BuildUserUpdatingInfo(user, oldUser).ToArray();
            if (!updatingInfo.Any())
            {
                return false;
            }

            _activeDirectoryService.Update(user.DistinguishedName, updatingInfo);
            UpdateDistinguishedName(user, oldUser);

            return true;
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

        public IEnumerable<User> GetUsersUpdatedBeetweenDates(DateTime startDate, DateTime endDate)
        {
            var filter = _filterBuildingService.BuildFilterForGettingUsersUpdatedBeetweenDates(
                startDate.ConvertToActiveDirectoryString(),
                endDate.ConvertToActiveDirectoryString());
            var result = _activeDirectoryService.FindEntities(_parentDistinguishedName, filter);
            return Mapper.Map<IEnumerable<User>>(result);
        }
    }
}