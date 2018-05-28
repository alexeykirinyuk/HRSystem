using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using LiteGuard;
using OneInc.ADEditor.ActiveDirectory;
using OneInc.ADEditor.ActiveDirectory.Extensions;
using OneInc.ADEditor.ActiveDirectory.Services.Requests.Interfaces;
using OneInc.ADEditor.Common.Utils;
using OneInc.ADEditor.Core.Log;
using OneInc.ADEditor.Core.Repositories;
using OneInc.ADEditor.Dal.Services.Interfaces;
using OneInc.ADEditor.FileStorage;
using OneInc.ADEditor.Models.Extensions;
using OneInc.ADEditor.PowerShell.Interfaces;
using OneInc.ADEditor.SharePoint;
using OneInc.ADEditor.SharePoint.Services.Interfaces;
using static OneInc.ADEditor.ActiveDirectory.ActiveDirectoryConstants;
using User = OneInc.ADEditor.Models.User;

namespace OneInc.ADEditor.Dal.Repositories
{
    public class UserRepository : IUserRepository
    {
        private const string CanUpdateDistingUishedNameKey = "CanUpdate";

        private readonly IActiveDirectoryFilterBuildingService _filterBuildingService;
        private readonly IActiveDirectoryDistinguishedNameBuilderService _distinguishedNameBuilderService;
        private readonly IActiveDirectoryService _activeDirectoryService;
        private readonly IActiveDirectoryUserCreationInfoBuilderService _creationInfoBuilderService;
        private readonly IActiveDirectoryUserUpdatingInfoBuilderService _updatingInfoBuilderService;
        private readonly ISharePointUserService _sharePointUserService;
        private readonly IPowerShellExchangeService _exchangeService;
        private readonly SharePointSettings _sharePointSettings;
        private readonly IStorageService _storageService;
        private readonly ILogger _logger;
        private readonly string _parentDistinguishedName;
        private readonly byte[] _defaultPhoto;

        private static readonly object LockObject = new object();
        private readonly string _canUpdateDistinguishedName;

        public UserRepository(
            IActiveDirectoryFilterBuildingService filterBuildingService,
            IActiveDirectoryDistinguishedNameBuilderService distinguishedNameBuilderService,
            IActiveDirectoryService activeDirectoryService,
            IActiveDirectoryUserCreationInfoBuilderService creationInfoBuilderService,
            IActiveDirectoryUserUpdatingInfoBuilderService updatingInfoBuilderService,
            ISharePointUserService sharePointUserService,
            IPowerShellExchangeService exchangeService,
            ActiveDirectorySettings activeDirectorySettings,
            SharePointSettings sharePointSettings,
            IStorageService storageService,
            ILoggerCreationService loggerCreationService)
        {
            Guard.AgainstNullArgument(nameof(filterBuildingService), filterBuildingService);
            Guard.AgainstNullArgument(nameof(distinguishedNameBuilderService), distinguishedNameBuilderService);
            Guard.AgainstNullArgument(nameof(activeDirectoryService), activeDirectoryService);
            Guard.AgainstNullArgument(nameof(creationInfoBuilderService), creationInfoBuilderService);
            Guard.AgainstNullArgument(nameof(updatingInfoBuilderService), updatingInfoBuilderService);
            Guard.AgainstNullArgument(nameof(sharePointUserService), sharePointUserService);
            Guard.AgainstNullArgument(nameof(exchangeService), exchangeService);
            Guard.AgainstNullArgument(nameof(sharePointSettings), sharePointSettings);
            Guard.AgainstNullArgument(nameof(activeDirectorySettings), activeDirectorySettings);
            Guard.AgainstNullArgument(nameof(storageService), storageService);
            Guard.AgainstNullArgument(nameof(loggerCreationService), loggerCreationService);

            _filterBuildingService = filterBuildingService;
            _distinguishedNameBuilderService = distinguishedNameBuilderService;
            _activeDirectoryService = activeDirectoryService;
            _creationInfoBuilderService = creationInfoBuilderService;
            _updatingInfoBuilderService = updatingInfoBuilderService;
            _sharePointUserService = sharePointUserService;
            _exchangeService = exchangeService;
            _sharePointSettings = sharePointSettings;
            _storageService = storageService;
            _logger = loggerCreationService.CreateLogger(GetType());

            _parentDistinguishedName = activeDirectorySettings.Paths[Entities.User];
            _canUpdateDistinguishedName = activeDirectorySettings.Paths[CanUpdateDistingUishedNameKey];
            _defaultPhoto = GetDefaultPhoto();
        }

        private static byte[] GetDefaultPhoto()
        {
            using (var stream =
                new FileStream($"{AppDomain.CurrentDomain.BaseDirectory}default-photo.png", FileMode.Open))
            {
                return stream.ReadAllBytes();
            }
        }

        public User GetByPrincipalName(string principalName)
        {
            var user = GetFlatUserByPrincipalName(principalName);
            if (user == null)
            {
                return null;
            }

            FillId(user);

            return user;
        }

        public User GetById(int id)
        {
            var userName = _sharePointUserService.GetUserNameById(id);
            var user = GetFlatUserByPrincipalName(userName);
            user.Id = id;

            return user;
        }

        private User GetFlatUserByPrincipalName(string principalName)
        {
            var filter = _filterBuildingService.BuildFilterForGettingUserByPrincipalName(principalName);
            var entity = _activeDirectoryService.Find(_parentDistinguishedName, filter);

            return Mapper.Map<User>(entity);
        }

        public User GetByDistinguishedName(string distinguishedName)
        {
            var filter =
                _filterBuildingService.BuildFilterForGettingByDistinguishedName(Entities.User, distinguishedName);
            var path = _filterBuildingService.BuildPathForGettingByDistinguishedName(distinguishedName);
            var result = _activeDirectoryService.Find(path, filter);
            var user = Mapper.Map<User>(result);

            if (user == null)
            {
                return null;
            }

            FillId(user);

            return user;
        }

        public IEnumerable<User> GetUsersByIds(IEnumerable<int> ids)
        {
            var hashSetIds = ids.Distinct().ToArray();
            if (!hashSetIds.Any())
            {
                return Enumerable.Empty<User>();
            }

            var items = _sharePointUserService.GetUserNamesByIds(hashSetIds);
            return GetActiveDirectoryUsersByPrincipalNames(items);
        }

        private IEnumerable<User> GetActiveDirectoryUsersByPrincipalNames(IDictionary<string, int> ids)
        {
            var filter = _filterBuildingService.BuildFilterForGettingUsersByUserPrincipalNames(ids.Keys);
            var entities = _activeDirectoryService.FindEntities(_parentDistinguishedName, filter);
            var result = Mapper.Map<IEnumerable<User>>(entities).ToArray();

            foreach (var user in result)
            {
                FindAndFillId(user, ids);
            }

            return result;
        }

        public string Create(User user, string password)
        {
            var attributes = _creationInfoBuilderService.BuilUserCreationInfo(user, password);
            var result = _activeDirectoryService.Create(
                user.Office.DistinguishedName,
                user.GetFullName(),
                attributes.ToArray());

            return result;
        }

        public bool UpdateIfNeeded(User user)
        {
            var oldUser = GetFlatUserByPrincipalName(user.PrincipalName);
            if (!oldUser.IsActive)
            {
                return false;
            }

            user.DistinguishedName = oldUser.DistinguishedName;
            if (!user.DistinguishedName.Contains(_canUpdateDistinguishedName))
            {
                return false;
            }

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
            var newName = user.GetFullName();
            if (NeedUpdateDistinguishedName(user, oldUser.GetFullName(), newName))
            {
                user.DistinguishedName = _activeDirectoryService.UpdateDistinguishedName(
                    user.DistinguishedName,
                    user.Office.DistinguishedName,
                    newName);
            }
        }

        private bool NeedUpdateDistinguishedName(User user, string oldName, string newName)
        {
            if (string.IsNullOrEmpty(user.Office?.DistinguishedName))
            {
                return false;
            }

            var parentDistinguishedName =
                _distinguishedNameBuilderService.GetParentDirectoryFromDistinguishedName(user.DistinguishedName);
            var officeNotEquals = user.Office.DistinguishedName != parentDistinguishedName;
            var nameNotEquals = newName != oldName;

            return officeNotEquals || nameNotEquals;
        }

        public IEnumerable<User> GetUsersUpdatedBeetweenDates(DateTime startDate, DateTime endDate)
        {
            var filter = _filterBuildingService.BuildFilterForGettingUsersUpdatedBeetweenDates(
                startDate.ConvertToActiveDirectoryString(),
                endDate.ConvertToActiveDirectoryString());
            var result = _activeDirectoryService.FindEntities(_parentDistinguishedName, filter);
            var users = Mapper.Map<IEnumerable<User>>(result).WhereIsCorrect();

            return FillIds(users);
        }

        private IEnumerable<User> FillIds(IEnumerable<User> users)
        {
            var userArray = users.ToArray();
            if (userArray.IsEmpty())
            {
                return userArray;
            }

            var principalNames = userArray.Select(u => u.PrincipalName);
            var ids = _sharePointUserService.GetIdsByUserNames(principalNames);

            foreach (var user in userArray)
            {
                FindAndFillId(user, ids);
            }

            return userArray.WhereIdIsNotNull();
        }

        private static void FindAndFillId(User user, IDictionary<string, int> ids)
        {
            if (ids.TryGetValue(user.PrincipalName, out var id, StringComparison.OrdinalIgnoreCase))
            {
                user.Id = id;
            }
        }

        public bool IsUserSynced(string email)
        {
            return _sharePointUserService.GetIdByEmail(email).HasValue;
        }

        public void FillId(User user)
        {
            user.Id = _sharePointUserService.GetIdByEmail(user.Email);
        }

        public IDictionary<string, int> GetIdsByEmails(IEnumerable<string> emails)
        {
            return _sharePointUserService.GetIdsByUserNames(emails.Distinct());
        }

        public string GetEmailDomain()
        {
            return _sharePointUserService.GetEmailDomain();
        }

        public bool ExistsUserWithSameAccountNameOrFullNameInOffice(
            string accountName,
            string officeDistinguishedName,
            string fullName)
        {
            var distinguishedName =
                _distinguishedNameBuilderService.BuildDistinguishedName(fullName, officeDistinguishedName);
            var filter = _filterBuildingService.BuildFilterForCheckingExistsUserWithSameDistinguishedNameOrAccountName(
                distinguishedName,
                accountName);

            var entities = _activeDirectoryService.FindEntities(_parentDistinguishedName, filter);

            return entities.Any();
        }

        public bool UpdatePhoto(string email, byte[] photoBytes)
        {
            lock (LockObject)
            {
                return UpdateExchangePhoto(email, photoBytes);
            }
        }

        private bool UpdateExchangePhoto(string email, byte[] photoBytes)
        {
            try
            {
                var path = GeneratePhotoPath(email);
                _storageService.CreateFile(path, photoBytes);
                _exchangeService.UpdateExchangePhoto(
                    _sharePointSettings.UserName,
                    _sharePointSettings.Password,
                    email,
                    path);
                _storageService.DeleteFile(path);

                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e, $"Error with updating exchange photo for {email}.");
            }

            return false;
        }

        private string GeneratePhotoPath(string email)
        {
            return $"{AppDomain.CurrentDomain.BaseDirectory}{_sharePointSettings.TempPhotoPath}/{email}.jpg";
        }

        public IEnumerable<User> GetAll()
        {
            var filter = _filterBuildingService.BuildFilterForGettingAllEntities(Entities.User);
            var entities = _activeDirectoryService.FindEntities(_parentDistinguishedName, filter);
            var users = Mapper.Map<IEnumerable<User>>(entities).WhereIsCorrect();

            return FillIds(users);
        }

        public IEnumerable<User> GetAllBlocked()
        {
            var filter = _filterBuildingService.BuildFilterForGettingAllBlockedUsers();
            var entities = _activeDirectoryService.FindEntities(_parentDistinguishedName, filter);

            return Mapper.Map<IEnumerable<User>>(entities).WhereNotNullAndNotEmpty(u => u.Email);
        }

        public IEnumerable<User> GetAllForSubscribe()
        {
            var filter = _filterBuildingService.BuildFilterForGettingAllEntities(Entities.User);
            var entities = _activeDirectoryService.FindEntities(_parentDistinguishedName, filter);

            return Mapper.Map<IEnumerable<User>>(entities).WhereIsCorrect();
        }

        public IDictionary<string, byte[]> GetUsersPhoto(IEnumerable<string> emails)
        {
            var emailArray = emails.ToArray();
            if (emailArray.IsEmpty())
            {
                return new Dictionary<string, byte[]>();
            }

            var tasks = GetUsersPhotoAsync(emailArray);
            tasks.Values.WaitAll();

            return tasks.CastValue(task => task.Result);
        }

        public IDictionary<string, Task<byte[]>> GetUsersPhotoAsync(IEnumerable<string> emails)
        {
            var client = CreateClient();
            return emails.ToDictionary(email => email, email => GetUserPhotoAsync(client, email));
        }

        public byte[] GetUserPhoto(string email)
        {
            var task = GetUserPhotoAsync(CreateClient(), email);
            Task.WaitAll(task);

            return task.Result;
        }

        private async Task<byte[]> GetUserPhotoAsync(HttpClient client, string email)
        {
            try
            {
                var url = string.Format(_sharePointSettings.AvatarUrlFormat, email);
                return await client.GetByteArrayAsync(url);
            }
            catch
            {
                return _defaultPhoto;
            }
        }

        private HttpClient CreateClient()
        {
            var handler = new HttpClientHandler
            {
                Credentials = new NetworkCredential(_sharePointSettings.UserName, _sharePointSettings.Password)
            };

            return new HttpClient(handler);
        }
    }
}