using System.Data.Entity;
using HRSystem.Models;
using HRSystem.Models.Attributes;

namespace HRSystem.Dal
{
    public partial class HRSystemDb
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasKey(employee => employee.Login);
            
            modelBuilder.Entity<AttributeBase>()
                .Property(attribute => attribute.Name)
                .IsRequired();
            modelBuilder.Entity<AttributeBase>()
                .Property(attribute => attribute.EmployeeLogin)
                .IsRequired();
            modelBuilder.Entity<AttributeBase>()
                .HasRequired(a => a.Employee)
                .WithMany(a => a.Attributes)
                .HasForeignKey(a => a.EmployeeLogin);

            modelBuilder.Entity<AttributeBase>().ToTable("Attributes");
            modelBuilder.Entity<DateTimeAttribute>().ToTable("DateTimeAttributes");
            modelBuilder.Entity<IntAttribute>().ToTable("IntAttributes");
            modelBuilder.Entity<StringAttribute>().ToTable("StringAttributes");
        }
    }
}