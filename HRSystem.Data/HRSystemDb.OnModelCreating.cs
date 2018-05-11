using HRSystem.Domain;
using HRSystem.Domain.Attributes;
using HRSystem.Domain.Attributes.Base;
using Microsoft.EntityFrameworkCore;

namespace HRSystem.Data
{
    public partial class HrSystemDb
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .HasKey(employee => employee.Login);
            
            modelBuilder.Entity<AttributeBase>()
                .Property(attribute => attribute.Name)
                .IsRequired();
            modelBuilder.Entity<AttributeBase>()
                .HasOne(attribute => attribute.Employee)
                .WithMany(employee => employee.Attributes)
                .HasForeignKey(attribute => attribute.EmployeeLogin)
                .IsRequired();
            

            modelBuilder.Entity<DateTimeAttribute>().HasBaseType<AttributeBase>();
            modelBuilder.Entity<IntAttribute>().HasBaseType<AttributeBase>();
            modelBuilder.Entity<StringAttribute>().HasBaseType<AttributeBase>();
        }
    }
}