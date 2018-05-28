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
    public class ProductRepository : IProductRepository
    {
        private readonly ISharePointListService _listService;
        private readonly ISharePointListQueryBuilderService _queryBuilderService;

        private readonly string _listName;

        public ProductRepository(
            ISharePointListService listService,
            ISharePointListQueryBuilderService queryBuilderService,
            SharePointSettings settings)
        {
            Guard.AgainstNullArgument(nameof(listService), listService);
            Guard.AgainstNullArgument(nameof(queryBuilderService), queryBuilderService);
            Guard.AgainstNullArgument(nameof(settings), settings);

            _listService = listService;
            _queryBuilderService = queryBuilderService;
            _listName = settings.ProductListName;
        }

        public Product GetById(int id)
        {
            var item = _listService.GetById(_listName, id);
            _listService.LoadAndExecute(item);

            return Mapper.Map<Product>(item);
        }

        public Product GetByName(string name)
        {
            var query = _queryBuilderService.BuildQueryForGettingByEqualsText(ProductAttributes.Name, name);
            var items = _listService.GetItems(_listName, query);
            _listService.LoadAndExecute(items);

            var item = items.FirstOrDefault();
            if (item == null)
            {
                item = CreateListItem(name);
            }

            return Mapper.Map<Product>(item);
        }

        public IEnumerable<Product> GetProductsByNames(IEnumerable<string> names)
        {
            var nameArray = names.Distinct().ToArray();
            var query = _queryBuilderService.BuildQueryForGettingByEqualsText(ProductAttributes.Name, nameArray);
            var items = _listService.GetItems(_listName, query);
            _listService.LoadAndExecute(items);

            var itemList = items.ToList(); 
            CreateNotContains(itemList, nameArray);

            return Mapper.Map<IEnumerable<Product>>(itemList);
        }

        private void CreateNotContains(List<ListItem> items, IEnumerable<string> names)
        {
            var itemNames = items.Select(item => item.GetStringValue(ProductAttributes.Name));
            var notExistsNames = names.WhereNot(name => itemNames.Contains(name));
            items.AddRange(notExistsNames.Select(CreateListItem));
        }

        private ListItem CreateListItem(string name)
        {
            var item = _listService.CreateItem(_listName);
            item[ProductAttributes.Name] = name;
            _listService.UpdateAndExecute(item);

            return item;
        }

        public IEnumerable<Product> GetProductsByIds(IEnumerable<int> ids)
        {
            var idArray = ids.Distinct().ToArray();
            if (!idArray.Any())
            {
                return Enumerable.Empty<Product>();
            }

            var items = _listService.GetByIds(_listName, idArray);
            foreach (var item in items)
            {
                _listService.Load(item);
            }

            _listService.Execute();

            return Mapper.Map<IEnumerable<Product>>(items);
        }
    }
}
