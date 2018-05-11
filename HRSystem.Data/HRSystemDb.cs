using HRSystem.Domain;
using HRSystem.Domain.Attributes;
using HRSystem.Domain.Attributes.Base;
using Microsoft.EntityFrameworkCore;

namespace HRSystem.Data
{
    public partial class HrSystemDb : DbContext
    {
        public HrSystemDb(DbContextOptions<HrSystemDb> contextOptions) : base(contextOptions)
        {
        }

        public DbSet<Employee> Employees { get; set; }

        #region Attributes

        public DbSet<AttributeBase> Attributes { get; set; }

        public DbSet<IntAttribute> IntAttributes { get; set; }

        public DbSet<StringAttribute> StringAttributes { get; set; }

        public DbSet<DateTimeAttribute> DateTimeAttributes { get; set; }

        #endregion
    }
}