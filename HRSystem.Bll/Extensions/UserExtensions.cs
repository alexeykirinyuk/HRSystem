using System.Collections.Generic;
using System.Linq;
using HRSystem.Domain;

namespace HRSystem.Bll.Extensions
{
    public static class UserExtensions
    {
        public static bool ContainsByLogin(this IEnumerable<Employee> employees, string login)
        {
            return employees.Any(u => u.Login == login);
        }
    }
}