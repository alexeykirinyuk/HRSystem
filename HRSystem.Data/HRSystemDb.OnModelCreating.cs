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

            modelBuilder.Entity<AttributeInfo>()
                .Property(info => info.Name)
                .IsRequired();
            modelBuilder.Entity<AttributeBase>()
                .HasOne(attribute => attribute.Employee)
                .WithMany(employee => employee.Attributes)
                .HasForeignKey(attribute => attribute.EmployeeLogin)
                .IsRequired();

            modelBuilder.Entity<AttributeBase>()
                .HasDiscriminator(attribute => attribute.Descriminator)
                .HasValue<DateTimeAttribute>(AttributeType.DateTime)
                .HasValue<IntAttribute>(AttributeType.Int)
                .HasValue<StringAttribute>(AttributeType.String);

            modelBuilder.Entity<DateTimeAttribute>()
                .ToTable("DateTimeAttribute");
            modelBuilder.Entity<IntAttribute>()
                .ToTable("IntAttribute");
            modelBuilder.Entity<StringAttribute>()
                .ToTable("StringAttribute");
        }
    }
}