using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using LiteGuard;
using Microsoft.SharePoint.Client;
using OneInc.ADEditor.Common.Utils;
using OneInc.ADEditor.Core.Repositories;
using OneInc.ADEditor.Dal.Extensions;
using OneInc.ADEditor.Models;
using OneInc.ADEditor.SharePoint;
using OneInc.ADEditor.SharePoint.Services.Interfaces;
using static OneInc.ADEditor.Dal.SharePointEntitiesConstants;

namespace OneInc.ADEditor.Dal.Repositories
{
    public class JobRepository : IJobRepository
    {
        private readonly ISharePointListService _listService;
        private readonly ISharePointListQueryBuilderService _queryBuilderService;

        private readonly string _listName;

        public JobRepository(
            ISharePointListService listService,
            ISharePointListQueryBuilderService queryBuilderService,
            SharePointSettings settings)
        {
            Guard.AgainstNullArgument(nameof(listService), listService);
            Guard.AgainstNullArgument(nameof(queryBuilderService), queryBuilderService);
            Guard.AgainstNullArgument(nameof(settings), settings);

            _listService = listService;
            _queryBuilderService = queryBuilderService;
            _listName = settings.JobListName;
        }

        public Job GetById(int id)
        {
            var item = _listService.GetById(_listName, id);
            _listService.LoadAndExecute(item);

            return Mapper.Map<Job>(item);
        }

        public Job GetOrCreateByTitle(string title)
        {
            var query = _queryBuilderService.BuildQueryForGettingByEqualsText(JobAttributes.Title, title);
            var items = _listService.GetItems(_listName, query);
            _listService.LoadAndExecute(items);

            var item = items.FirstOrDefault();
            if (item == null)
            {
                item = CreateListItem(title);
            }

            return Mapper.Map<Job>(item);
        }

        private ListItem CreateListItem(string name)
        {
            var item = _listService.CreateItem(_listName);
            item[JobAttributes.Title] = name;
            _listService.UpdateAndExecute(item);

            return item;
        }

        public IEnumerable<Job> GetJobsByIds(IEnumerable<int> ids)
        {
            var idArray = ids.Distinct().ToArray();
            if (!idArray.Any())
            {
                return Enumerable.Empty<Job>();
            }

            var items = _listService.GetByIds(_listName, idArray);
            foreach (var item in items)
            {
                _listService.Load(item);
            }

            _listService.Execute();

            return Mapper.Map<IEnumerable<Job>>(items);
        }

        public IEnumerable<Job> GetJobsByTitles(IEnumerable<string> titles)
        {
            var titleArray = titles.Distinct().ToArray();
            var query = _queryBuilderService.BuildQueryForGettingByEqualsText(JobAttributes.Title, titleArray);
            var items = _listService.GetItems(_listName, query);
            _listService.LoadAndExecute(items);

            var itemList = items.ToList();
            CreateNotContains(itemList, titleArray);

            return Mapper.Map<IEnumerable<Job>>(itemList);
        }

        private void CreateNotContains(List<ListItem> items, IEnumerable<string> titles)
        {
            var itemTitles = items.Select(item => item.GetStringValue(JobAttributes.Title));
            var notExistsTitles = titles.WhereNot(name => itemTitles.Contains(name));
            items.AddRange(notExistsTitles.Select(CreateListItem));
        }
    }
}
