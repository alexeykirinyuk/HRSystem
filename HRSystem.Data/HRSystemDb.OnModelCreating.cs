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
                .Property(e => e.FirstName)
                .IsRequired();
            modelBuilder.Entity<Employee>()
                .Property(e => e.LastName)
                .IsRequired();
            modelBuilder.Entity<Employee>()
                .Property(e => e.Login)
                .IsRequired();
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
                .HasValue<StringAttribute>(AttributeType.String)
                .HasValue<BoolAttribute>(AttributeType.Bool);

            modelBuilder.Entity<DateTimeAttribute>()
                .Property(d => d.Value)
                .HasColumnName("DateTimeAttributeValue");
            modelBuilder.Entity<IntAttribute>()
                .Property(d => d.Value)
                .HasColumnName("IntAttributeValue");
            modelBuilder.Entity<StringAttribute>()
                .Property(d => d.Value)
                .HasColumnName("StringAttributeValue");
            modelBuilder.Entity<BoolAttribute>()
                .Property(d => d.Value)
                .HasColumnName("BoolAttributeValue");
        }
    }
}