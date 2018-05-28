using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using LiteGuard;
using Microsoft.SharePoint.Client;
using NLog;
using NLog.Fluent;
using OneInc.ADEditor.Common.Utils;
using OneInc.ADEditor.Core.Repositories;
using OneInc.ADEditor.Dal.Extensions;
using OneInc.ADEditor.Models;
using OneInc.ADEditor.Models.Extensions;
using OneInc.ADEditor.SharePoint;
using OneInc.ADEditor.SharePoint.Services.Interfaces;
using static OneInc.ADEditor.Dal.SharePointEntitiesConstants;

namespace OneInc.ADEditor.Dal.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ISharePointListQueryBuilderService _queryBuilderService;
        private readonly ISharePointListService _listService;
        private readonly string _listName;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly string[] _fields =
        {
            EmployeeAttributes.Photo,
            EmployeeAttributes.FirstName,
            EmployeeAttributes.LastName,
            EmployeeAttributes.Email,
            EmployeeAttributes.User,
            EmployeeAttributes.Office,
            EmployeeAttributes.Manager,
            EmployeeAttributes.Department,
            EmployeeAttributes.Job,
            EmployeeAttributes.Phone,
            EmployeeAttributes.WhenUpdated
        };

        public EmployeeRepository(
            ISharePointListQueryBuilderService queryBuilderService,
            ISharePointListService listService,
            SharePointSettings settings)
        {
            Guard.AgainstNullArgument(nameof(queryBuilderService), queryBuilderService);
            Guard.AgainstNullArgument(nameof(listService), listService);
            Guard.AgainstNullArgument(nameof(settings), settings);

            _queryBuilderService = queryBuilderService;
            _listService = listService;
            _listName = settings.EmployeeListName;
        }

        public IEnumerable<Employee> GetAll()
        {
            var items = _listService.GetAll(_listName);
            _listService.LoadAndExecute(items);

            return Mapper.Map<IEnumerable<Employee>>(items);
        }

        public Employee GetById(int id)
        {
            var item = _listService.GetById(_listName, id);
            _listService.LoadAndExecute(item);

            return Mapper.Map<Employee>(item);
        }

        public Employee GetByEmail(string email)
        {
            var item = GetItemByEmail(email);
            return Mapper.Map<Employee>(item);
        }

        public IEnumerable<Employee> GetEmployeesUpdatedBeetveenDates(DateTime startDate, DateTime endDate)
        {
            var query = _queryBuilderService.BuildQueryForGettingRangeDate(
                EmployeeAttributes.WhenUpdated, 
                startDate,
                endDate, 
                _fields);
            var items = _listService.GetItems(_listName, query);
            _listService.LoadAndExecute(items);

            return Mapper.Map<IEnumerable<Employee>>(items);
        }

        public void Create(Employee employee)
        {
            var item = _listService.CreateItem(_listName);
            Mapper.Map(employee, item);
            _listService.UpdateAndExecute(item);
        }

        public bool UpdateIfNeeded(Employee employee)
        {
            var oldItem = GetItemByEmail(employee.Email);
            var updateItem = _listService.GetById(_listName, oldItem.Id);
            CollectUpdates(employee, oldItem, updateItem);

            return UpdateIfChanged(employee.Email, oldItem, updateItem);
        }

        private void CollectUpdates(Employee employee, ListItem oldItem, ListItem updateItem)
        {
            updateItem.UpdateUrlIfChanged(EmployeeAttributes.Photo, employee.Photo, oldItem);
            updateItem.UpdateUrlIfChanged(EmployeeAttributes.PhotoSmall, employee.PhotoSmall, oldItem);
            updateItem.UpdateUrlIfChanged(EmployeeAttributes.PhotoLarge, employee.PhotoLarge, oldItem);
            if (!string.Equals(employee.Email, oldItem.GetValue<string>(EmployeeAttributes.Email), StringComparison.OrdinalIgnoreCase))
            {
                updateItem[EmployeeAttributes.Email] = employee.Email;
            }
            
            updateItem.UpdateStringIfChanged(EmployeeAttributes.Name, employee.Name, oldItem);
            updateItem.UpdateStringIfChanged(EmployeeAttributes.FirstName, employee.FirstName, oldItem);
            updateItem.UpdateStringIfChanged(EmployeeAttributes.LastName, employee.LastName, oldItem);
            updateItem.UpdateLookupIfChanged(EmployeeAttributes.Department, employee.Department, oldItem);
            updateItem.UpdateLookupIfChanged(EmployeeAttributes.Job, employee.Job, oldItem);
            updateItem.UpdateStringIfChanged(EmployeeAttributes.Phone, employee.Phone, oldItem);
            updateItem.UpdateLookupIfChanged(EmployeeAttributes.Office, employee.Office, oldItem);
            updateItem.UpdateLookupIfChanged(EmployeeAttributes.Manager, employee.Manager, oldItem);
        }

        public void UpdatePhoto(string email, string photoUrl, string photoSmallUrl, string photoLargeUrl)
        {
            var oldItem = GetItemByEmail(email);
            var updateItem = _listService.GetById(_listName, oldItem.Id);
            updateItem.UpdateUrlIfChanged(EmployeeAttributes.Photo, photoUrl, oldItem);
            updateItem.UpdateUrlIfChanged(EmployeeAttributes.PhotoSmall, photoSmallUrl, oldItem);
            updateItem.UpdateUrlIfChanged(EmployeeAttributes.PhotoLarge, photoLargeUrl, oldItem);

            UpdateIfChanged(email, oldItem, updateItem);
        }

        public bool FireIfNeeded(string email)
        {
            var oldItem = GetItemByEmail(email);
            var updateItem = _listService.GetById(_listName, oldItem.Id);
            updateItem.UpdateValueIfChanged(EmployeeAttributes.Fired, true, oldItem);
            updateItem.UpdateLookupIfChanged(EmployeeAttributes.User, null, oldItem);

            return UpdateIfChanged(email, oldItem, updateItem);
        }

        private bool UpdateIfChanged(string employeeEmail, ListItem oldItem, ListItem updateItem)
        {
            if (updateItem.FieldValues.IsEmpty())
            {
                return false;
            }

            _listService.UpdateAndExecute(updateItem);
            LogChanges(employeeEmail, oldItem, updateItem);

            return true;
        }

        private void LogChanges(string employeeEmail, ListItem oldItem, ListItem updateItem)
        {
            var logMessageBuilder = new StringBuilder();
            logMessageBuilder.AppendFormat("{0} updated. Attributes: ", employeeEmail);
            foreach (var field in updateItem.FieldValues)
            {
                var oldItemValue = oldItem.FieldValues.GetValueOrDefault(field.Key);
                logMessageBuilder.AppendFormat("{0} from {1} to {2}, ", field.Key, oldItemValue, field.Value);
            }

            _logger.Info(logMessageBuilder.ToString());
        }

        private ListItem GetItemByEmail(string email)
        {
            var query = _queryBuilderService.BuildQueryForGettingByEquals(EmployeeAttributes.Email, email);
            var items = _listService.GetItems(_listName, query);
            _listService.LoadAndExecute(items);

            return items.FirstOrDefault();
        }
    }
}