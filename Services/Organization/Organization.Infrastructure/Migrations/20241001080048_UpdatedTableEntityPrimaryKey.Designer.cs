﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Organization.Infrastructure;

#nullable disable

namespace Organization.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241001080048_UpdatedTableEntityPrimaryKey")]
    partial class UpdatedTableEntityPrimaryKey
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Organization.Domain.Entities.Branch", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(6)
                        .HasColumnType("nvarchar(6)");

                    b.Property<TimeOnly>("Closing")
                        .HasColumnType("time");

                    b.Property<string>("CompanyId")
                        .IsRequired()
                        .HasColumnType("nvarchar(5)");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(120)
                        .HasColumnType("nvarchar(120)");

                    b.Property<bool>("Is24Hour")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset>("LastModified")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("LastModifiedBy")
                        .HasMaxLength(120)
                        .HasColumnType("nvarchar(120)");

                    b.Property<TimeOnly>("Opening")
                        .HasColumnType("time");

                    b.Property<decimal>("ServiceFee")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("Branches");
                });

            modelBuilder.Entity("Organization.Domain.Entities.Company", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(120)
                        .HasColumnType("nvarchar(120)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("LastModified")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("LastModifiedBy")
                        .HasMaxLength(120)
                        .HasColumnType("nvarchar(120)");

                    b.Property<string>("LogoUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("Organization.Domain.Entities.Table", b =>
                {
                    b.Property<string>("BranchId")
                        .HasMaxLength(6)
                        .HasColumnType("nvarchar(6)");

                    b.Property<string>("TableNumber")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<decimal>("Deposit")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("BranchId", "TableNumber");

                    b.ToTable("Tables");
                });

            modelBuilder.Entity("Organization.Domain.Entities.Branch", b =>
                {
                    b.HasOne("Organization.Domain.Entities.Company", "Company")
                        .WithMany("Branches")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.OwnsOne("Organization.Domain.ValueObjects.Address", "Address", b1 =>
                        {
                            b1.Property<string>("BranchId")
                                .HasColumnType("nvarchar(6)");

                            b1.Property<string>("City")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Country")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("GoogleMapsEmbed")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("GoogleMapsLocation")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Region")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Street")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("BranchId");

                            b1.ToTable("Branches");

                            b1.WithOwner()
                                .HasForeignKey("BranchId");
                        });

                    b.Navigation("Address")
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("Organization.Domain.Entities.Table", b =>
                {
                    b.HasOne("Organization.Domain.Entities.Branch", "Branch")
                        .WithMany("Tables")
                        .HasForeignKey("BranchId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Branch");
                });

            modelBuilder.Entity("Organization.Domain.Entities.Branch", b =>
                {
                    b.Navigation("Tables");
                });

            modelBuilder.Entity("Organization.Domain.Entities.Company", b =>
                {
                    b.Navigation("Branches");
                });
#pragma warning restore 612, 618
        }
    }
}