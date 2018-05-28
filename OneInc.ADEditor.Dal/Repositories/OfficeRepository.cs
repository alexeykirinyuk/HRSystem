using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Linq;
using LiteGuard;
using Microsoft.SharePoint.Client;
using OneInc.ADEditor.ActiveDirectory;
using OneInc.ADEditor.ActiveDirectory.Extensions;
using OneInc.ADEditor.ActiveDirectory.Services.Requests.Interfaces;
using OneInc.ADEditor.Core.Repositories;
using OneInc.ADEditor.Dal.Extensions;
using OneInc.ADEditor.Models;
using OneInc.ADEditor.SharePoint;
using OneInc.ADEditor.SharePoint.Services.Interfaces;
using static OneInc.ADEditor.ActiveDirectory.ActiveDirectoryConstants;
using static OneInc.ADEditor.Dal.SharePointEntitiesConstants;

namespace OneInc.ADEditor.Dal.Repositories
{
    public class OfficeRepository : IOfficeRepository
    {
        private readonly IActiveDirectoryService _activeDirectoryService;
        private readonly IActiveDirectoryFilterBuildingService _filterBuildingService;
        private readonly ISharePointListQueryBuilderService _queryBuilderService;
        private readonly ISharePointListService _listService;

        private readonly string _listName;
        private readonly string _parentDistinguishedName;
        private readonly ActiveDirectorySettings _settings;

        public OfficeRepository(
            IActiveDirectoryService activeDirectoryService,
            IActiveDirectoryFilterBuildingService filterBuildingService,
            ISharePointListQueryBuilderService queryBuilderService,
            ISharePointListService listService,
            SharePointSettings sharePointSettings,
            ActiveDirectorySettings activeDirectorySettings)
        {
            Guard.AgainstNullArgument(nameof(activeDirectoryService), activeDirectoryService);
            Guard.AgainstNullArgument(nameof(filterBuildingService), filterBuildingService);
            Guard.AgainstNullArgument(nameof(queryBuilderService), queryBuilderService);
            Guard.AgainstNullArgument(nameof(listService), listService);
            Guard.AgainstNullArgument(nameof(sharePointSettings), sharePointSettings);
            Guard.AgainstNullArgument(nameof(activeDirectorySettings), activeDirectorySettings);

            _activeDirectoryService = activeDirectoryService;
            _filterBuildingService = filterBuildingService;
            _queryBuilderService = queryBuilderService;
            _listService = listService;
            _settings = activeDirectorySettings;

            _listName = sharePointSettings.OfficeListName;
            _parentDistinguishedName = activeDirectorySettings.Paths[Entities.Office];
        }

        public Office GetByLocation(string location)
        {
            var filter = _filterBuildingService.BuildFilterForGettingOfficeByLocation(PrepareLocation(location));
            var organizationUnit = _activeDirectoryService.FindEntities(_parentDistinguishedName, filter);

            var query = _queryBuilderService.BuildQueryForGettingByEquals(OfficeAttributes.Location, location);
            var itemCollection = _listService.GetItems(_listName, query);
            _listService.LoadAndExecute(itemCollection);

            var items = itemCollection.ToList();
            return GenerateOffice(location, organizationUnit, items);
        }

        public IEnumerable<Office> GetOfficesByLocations(IEnumerable<string> locations)
        {
            var locationArray = locations.Distinct().ToArray();
            if (!locationArray.Any())
            {
                return Enumerable.Empty<Office>();
            }

            var filter = _filterBuildingService.BuildFilterForGettingOfficesByLocations(locationArray.Select(PrepareLocation));
            var organizationUnits = _activeDirectoryService.FindEntities(_parentDistinguishedName, filter);

            var query = _queryBuilderService.BuildQueryForGettingByEqualsText(OfficeAttributes.Location, locationArray.ToArray());
            var itemCollection = _listService.GetItems(_listName, query);
            _listService.LoadAndExecute(itemCollection);

            var items = itemCollection.ToList();
            return locationArray.Select(location => GenerateOffice(location, organizationUnits, items));
        }

        private Office GenerateOffice(string location, IEnumerable<SearchResultEntry> organizationUnits, ICollection<ListItem> items)
        {
            var preparedLocation = PrepareLocation(location);

            var organizationUnit = organizationUnits.FirstOrDefault(o => o.GetPropertyValue(EntityAttributes.Location) == preparedLocation);
            var item = items.FirstOrDefault(it => it.GetStringValue(OfficeAttributes.Location) == location);
            if (item == null)
            {
                item = CreateListItem(location);
                items.Add(item);
            }

            return new Office
            {
                Id = item?.Id,
                DistinguishedName = PrepareDistinguishedName(organizationUnit?.DistinguishedName),
                Location = location
            };
        }

        private ListItem CreateListItem(string location)
        {
            var item = _listService.CreateItem(_listName);
            item[OfficeAttributes.Location] = location;
            _listService.UpdateAndExecute(item);

            return item;
        }

        private string PrepareDistinguishedName(string distinguishedName)
        {
            if (string.IsNullOrEmpty(distinguishedName))
            {
                return null;
            }

            return $"{_settings.UserCreationPathPrefix},{distinguishedName}";
        }

        private static string PrepareLocation(string location)
        {
            return location.Split(',').FirstOrDefault() ?? string.Empty;
        }
    }
}
