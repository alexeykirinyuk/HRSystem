using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using HRSystem.Domain;
using Microsoft.EntityFrameworkCore;

namespace HRSystem.Data
{
    public static class EmployeeRepository
    {
        public static Task<Employee> GetByLogin(this IQueryable<Employee> queryable, string login)
        {
            return queryable.SingleAsync(e => e.Login == login);
        }

        public static Task<bool> IsExists(this IQueryable<Employee> queryable, string login)
        {
            return queryable.AnyAsync(e => e.Login == login);
        }

        public static IQueryable<Employee> IncludeAll(this IQueryable<Employee> queryable)
        {
            return queryable.Include(e => e.Manager).Include(e => e.Attributes);
        }

        public static IQueryable<Employee> GetEmployeesUpdatedFrom(this IQueryable<Employee> queryable, DateTime @from)
        {
            return queryable.Where(e => e.LastModified > from);
        }

        public static IQueryable<Employee> Search(this IQueryable<Employee> queryable, string searchFilter)
        {
            return queryable
                .Where(e => e.Login.Contains(searchFilter) ||
                            e.FullName.Contains(searchFilter) ||
                            e.Email != null && e.Email.Contains(searchFilter) ||
                            e.JobTitle != null && e.JobTitle.Contains(searchFilter) ||
                            e.Phone != null && e.Phone.Contains(searchFilter) ||
                            e.Office != null && e.Office.Contains(searchFilter) ||
                            e.Manager != null && e.Manager.FullName.Contains(searchFilter) ||
                            e.Attributes.Any(a => a.GetValueAsString().Contains(searchFilter)));
        }

        public static IQueryable<Employee> SearchByManager(this IQueryable<Employee> queryable, string managerFullName)
        {
            return queryable
                .Where(e => e.Manager != null)
                .Where(e => e.Manager.FullName.Contains(managerFullName));
        }

        public static IQueryable<Employee> SearchByOffice(this IQueryable<Employee> queryable, string office)
        {
            return queryable
                .Where(e => e.Office.Contains(office));
        }

        public static IQueryable<Employee> SearchByJobTitle(this IQueryable<Employee> queryable, string office)
        {
            return queryable
                .Where(e => e.JobTitle.Contains(office));
        }

        public static IQueryable<Employee> SearchByAttribute(
            this IQueryable<Employee> queryable,
            int attributeInfoId,
            string value)
        {
            return queryable
                .Where(
                    e => e.Attributes.Any(a =>
                        a.AttributeInfoId == attributeInfoId &&
                        a.GetValueAsString().Contains(value)));
        }
    }
}