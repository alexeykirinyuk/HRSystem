using System.Data.Entity;
using HRSystem.Models;

namespace HRSystem.Dal
{
    public partial class HRSystemDb: DbContext
    {
        public HRSystemDb(): base("HRSystemDb")
        {
        }
        
        public DbSet<Employee> Employees { get; set; }
        
        public DbSet<AttributeBase> AttributeBases { get; set; }
    }
}