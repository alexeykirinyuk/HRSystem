using HRSystem.Domain;
using HRSystem.Domain.Attributes;
using HRSystem.Domain.Attributes.Base;
using Microsoft.EntityFrameworkCore;

namespace HRSystem.Data
{
    public partial class HrSystemDb : DbContext
    {
        public HrSystemDb(DbContextOptions contextOptions) : base(contextOptions)
        {
        }

        public DbSet<Employee> Employees { get; set; }

        #region Attributes
        
        public DbSet<AttributeInfo> AttributeInfos { get; set; }
        
        public DbSet<ActiveDirectoryAttributeInfo> ActiveDirectoryAttributeInfos { get; set; }
        
        public DbSet<BoolAttribute> BoolAttributes { get; set; }

        public DbSet<IntAttribute> IntAttributes { get; set; }

        public DbSet<StringAttribute> StringAttributes { get; set; }

        public DbSet<DateTimeAttribute> DateTimeAttributes { get; set; }
        
        public DbSet<AttributeBase> AttributeBases { get; set; }

        #endregion
    }
}