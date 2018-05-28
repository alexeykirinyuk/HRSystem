using System.DirectoryServices.Protocols;
using System.Net;
using HRSystem.Core;
using HRSystem.Domain;
using OneInc.ADEditor.ActiveDirectory;
using OneInc.ADEditor.ActiveDirectory.Services.Requests.Interfaces;

namespace HRSystem.ActiveDirectory
{
    public class UserService : IUserService
    {
        private readonly IActiveDirectoryService _service;
        private readonly IActiveDirectoryFilterBuildingService _filterService;
        private readonly ActiveDirectorySettings _activeDirectorySettings;

        public UserService(
            IActiveDirectoryService service, 
            IActiveDirectoryFilterBuildingService filterService,
            ActiveDirectorySettings activeDirectorySettings)
        {
            _service = service;
            _filterService = filterService;
            _activeDirectorySettings = activeDirectorySettings;
        }

        public void CreateUser(Employee employee)
        {
            var filterGetOfficeByLocation = _filterService.BuildFilterForGettingOfficeByLocation(employee.Office);
            var officeOrganizationUnit = _service.Find(_activeDirectorySettings.Paths["Office"], filterGetOfficeByLocation, SearchScope.OneLevel);
            
        }

        public void UpdateUser(Employee employee)
        {
            
        }
    }
}