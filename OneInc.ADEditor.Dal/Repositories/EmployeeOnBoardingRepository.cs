using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using LiteGuard;
using OneInc.ADEditor.Core.Repositories;
using OneInc.ADEditor.Models;
using OneInc.ADEditor.SharePoint;
using OneInc.ADEditor.SharePoint.Services.Interfaces;
using static OneInc.ADEditor.Dal.SharePointEntitiesConstants;

namespace OneInc.ADEditor.Dal.Repositories
{
    public class EmployeeOnBoardingRepository : IEmployeeOnBoardingRepository
    {
        private readonly ISharePointListService _listService;
        private readonly ISharePointListQueryBuilderService _queryBuilderService;
        private readonly string _listName;

        public EmployeeOnBoardingRepository(
            ISharePointListService listService,
            ISharePointListQueryBuilderService queryBuilderService,
            SharePointSettings settings)
        {
            Guard.AgainstNullArgument(nameof(listService), listService);
            Guard.AgainstNullArgument(nameof(queryBuilderService), queryBuilderService);
            Guard.AgainstNullArgument(nameof(settings), settings);

            _listService = listService;
            _queryBuilderService = queryBuilderService;
            _listName = settings.EmployeeOnBoardingListName;
        }

        public EmployeeOnBoarding GetById(int id)
        {
            var item = _listService.GetById(_listName, id);
            _listService.Load(item);
            _listService.Execute();

            return Mapper.Map<EmployeeOnBoarding>(item);
        }

        public EmployeeOnBoarding GetByEmail(string email)
        {
            var camlQuery = _queryBuilderService.BuildQueryForGettingByEqualsText(EmployeeAttributes.Email, email);
            var items = _listService.GetItems(_listName, camlQuery);
            _listService.LoadAndExecute(items);

            return Mapper.Map<EmployeeOnBoarding>(items.FirstOrDefault());
        }

        public IEnumerable<EmployeeOnBoarding> GetActiveDirectorySyncedEmployees()
        {
            var camlQuery = _queryBuilderService.BuildQueryForGettingByEquals(EmployeeAttributes.ActiveDirectorySynced, "1", "Boolean");
            var items = _listService.GetItems(_listName, camlQuery);
            _listService.LoadAndExecute(items);

            return Mapper.Map<IEnumerable<EmployeeOnBoarding>>(items);
        }

        public IEnumerable<EmployeeOnBoarding> GetAdminApprovedAndNotSyncedEmployees()
        {
            var camlQuery = _queryBuilderService.BuildQueryForGettingByEqualsBoolean(
                EmployeeAttributes.AdminApproved,
                "1",
                EmployeeAttributes.ActiveDirectorySynced,
                "0");
            var items = _listService.GetItems(_listName, camlQuery);
            _listService.LoadAndExecute(items);

            return Mapper.Map<IEnumerable<EmployeeOnBoarding>>(items);
        }

        public void UpdateEmail(int id, string email)
        {
            var item = _listService.GetById(_listName, id);
            _listService.LoadAndExecute(item, employee => employee[EmployeeAttributes.Email]);
            item[EmployeeAttributes.Email] = email;

            _listService.UpdateAndExecute(item);
        }

        public void UpdateActiveDirectorySynced(int id, bool synced)
        {
            var item = _listService.GetById(_listName, id);
            _listService.LoadAndExecute(item, employee => employee[EmployeeAttributes.ActiveDirectorySynced]);
            item[EmployeeAttributes.ActiveDirectorySynced] = synced;

            _listService.UpdateAndExecute(item);
        }

        public void UpdateSharePointSynced(int id, bool synced)
        {
            var item = _listService.GetById(_listName, id);
            _listService.LoadAndExecute(item, employee => employee[EmployeeAttributes.SharePointSynced]);
            item[EmployeeAttributes.SharePointSynced] = synced;

            _listService.UpdateAndExecute(item);
        }

        public void UpdateError(int id, string error)
        {
            var item = _listService.GetById(_listName, id);
            _listService.LoadAndExecute(item, employee => employee[EmployeeAttributes.Errors]);
            item[EmployeeAttributes.Errors] = error;

            _listService.UpdateAndExecute(item);
        }

        public void Delete(int id)
        {
            var item = _listService.GetById(_listName, id);
            _listService.LoadAndExecute(item, employee => employee.Id);

            _listService.DeleteAndExecute(item);
        }
    }
}
