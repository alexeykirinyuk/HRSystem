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
    }
}