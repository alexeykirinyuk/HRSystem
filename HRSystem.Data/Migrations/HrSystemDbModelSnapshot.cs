﻿// <auto-generated />
using System;
using HRSystem.Data;
using HRSystem.Domain.Attributes.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HRSystem.Data.Migrations
{
    [DbContext(typeof(HrSystemDb))]
    partial class HrSystemDbModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.0-rc1-32029")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("HRSystem.Domain.Attributes.Base.ActiveDirectoryAttributeInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("ActiveDirectoryAttributeInfos");
                });

            modelBuilder.Entity("HRSystem.Domain.Attributes.Base.AttributeBase", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AttributeInfoId");

                    b.Property<int>("Descriminator");

                    b.Property<string>("EmployeeLogin")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("AttributeInfoId");

                    b.HasIndex("EmployeeLogin");

                    b.ToTable("AttributeBases");

                    b.HasDiscriminator<int>("Descriminator");
                });

            modelBuilder.Entity("HRSystem.Domain.Attributes.Base.AttributeInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ActiveDirectoryAttributeInfoId");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.HasIndex("ActiveDirectoryAttributeInfoId");

                    b.ToTable("AttributeInfos");
                });

            modelBuilder.Entity("HRSystem.Domain.Employee", b =>
                {
                    b.Property<string>("Login")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<string>("JobTitle");

                    b.Property<string>("LastName");

                    b.Property<string>("ManagerLogin");

                    b.Property<string>("Office");

                    b.Property<string>("Phone");

                    b.HasKey("Login");

                    b.HasIndex("ManagerLogin");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("HRSystem.Domain.Attributes.BoolAttribute", b =>
                {
                    b.HasBaseType("HRSystem.Domain.Attributes.Base.AttributeBase");

                    b.Property<bool?>("Value")
                        .HasColumnName("BoolAttributeValue");

                    b.ToTable("BoolAttribute");

                    b.HasDiscriminator().HasValue(4);
                });

            modelBuilder.Entity("HRSystem.Domain.Attributes.DateTimeAttribute", b =>
                {
                    b.HasBaseType("HRSystem.Domain.Attributes.Base.AttributeBase");

                    b.Property<DateTime?>("Value")
                        .HasColumnName("DateTimeAttributeValue");

                    b.ToTable("DateTimeAttribute");

                    b.HasDiscriminator().HasValue(2);
                });

            modelBuilder.Entity("HRSystem.Domain.Attributes.IntAttribute", b =>
                {
                    b.HasBaseType("HRSystem.Domain.Attributes.Base.AttributeBase");

                    b.Property<int?>("Value")
                        .HasColumnName("IntAttributeValue");

                    b.ToTable("IntAttribute");

                    b.HasDiscriminator().HasValue(0);
                });

            modelBuilder.Entity("HRSystem.Domain.Attributes.StringAttribute", b =>
                {
                    b.HasBaseType("HRSystem.Domain.Attributes.Base.AttributeBase");

                    b.Property<string>("Value")
                        .HasColumnName("StringAttributeValue");

                    b.ToTable("StringAttribute");

                    b.HasDiscriminator().HasValue(1);
                });

            modelBuilder.Entity("HRSystem.Domain.Attributes.Base.AttributeBase", b =>
                {
                    b.HasOne("HRSystem.Domain.Attributes.Base.AttributeInfo", "AttributeInfo")
                        .WithMany()
                        .HasForeignKey("AttributeInfoId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("HRSystem.Domain.Employee", "Employee")
                        .WithMany("Attributes")
                        .HasForeignKey("EmployeeLogin")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("HRSystem.Domain.Attributes.Base.AttributeInfo", b =>
                {
                    b.HasOne("HRSystem.Domain.Attributes.Base.ActiveDirectoryAttributeInfo", "ActiveDirectoryAttributeInfo")
                        .WithMany()
                        .HasForeignKey("ActiveDirectoryAttributeInfoId");
                });

            modelBuilder.Entity("HRSystem.Domain.Employee", b =>
                {
                    b.HasOne("HRSystem.Domain.Employee", "Manager")
                        .WithMany()
                        .HasForeignKey("ManagerLogin");
                });
#pragma warning restore 612, 618
        }
    }
}
