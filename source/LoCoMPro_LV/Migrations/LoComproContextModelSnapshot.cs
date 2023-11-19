﻿// <auto-generated />
using System;
using LoCoMPro_LV.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LoCoMPro_LV.Migrations
{
    [DbContext(typeof(LoComproContext))]
    partial class LoComproContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("LoCoMPro_LV.Models.Anomalie", b =>
                {
                    b.Property<string>("NameGenerator")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("RecordDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Type")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Comment")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.HasKey("NameGenerator", "RecordDate", "Type");

                    b.ToTable("Anomalies", (string)null);
                });

            modelBuilder.Entity("LoCoMPro_LV.Models.ApplicationUser", b =>
                {
                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("IsModerator")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<double>("Latitude")
                        .HasColumnType("float");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<double>("Longitude")
                        .HasColumnType("float");

                    b.Property<string>("NameCanton")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("NameProvince")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserName");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.HasIndex("NameProvince", "NameCanton");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("LoCoMPro_LV.Models.Associated", b =>
                {
                    b.Property<string>("NameProduct")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("NameCategory")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("NameProduct", "NameCategory");

                    b.HasIndex("NameCategory");

                    b.ToTable("Associated", (string)null);
                });

            modelBuilder.Entity("LoCoMPro_LV.Models.Canton", b =>
                {
                    b.Property<string>("NameProvince")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("NameCanton")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("NameProvince", "NameCanton");

                    b.ToTable("Cantons", (string)null);
                });

            modelBuilder.Entity("LoCoMPro_LV.Models.Category", b =>
                {
                    b.Property<string>("NameCategory")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("NameTopCategory")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("NameCategory");

                    b.HasIndex("NameTopCategory");

                    b.ToTable("Category", (string)null);
                });

            modelBuilder.Entity("LoCoMPro_LV.Models.Evaluate", b =>
                {
                    b.Property<string>("NameGenerator")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("RecordDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("NameEvaluator")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("StarsCount")
                        .HasColumnType("int");

                    b.HasKey("NameGenerator", "RecordDate", "NameEvaluator");

                    b.HasIndex("NameEvaluator");

                    b.ToTable("Valorations", (string)null);
                });

            modelBuilder.Entity("LoCoMPro_LV.Models.GeneratorUser", b =>
                {
                    b.Property<string>("UserName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("UserName");

                    b.ToTable("GeneratorUser", (string)null);
                });

            modelBuilder.Entity("LoCoMPro_LV.Models.ModeratorUser", b =>
                {
                    b.Property<string>("UserName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("UserName");

                    b.ToTable("ModeratorUser", (string)null);
                });

            modelBuilder.Entity("LoCoMPro_LV.Models.Product", b =>
                {
                    b.Property<string>("NameProduct")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("NameProduct");

                    b.ToTable("Product", (string)null);
                });

            modelBuilder.Entity("LoCoMPro_LV.Models.Province", b =>
                {
                    b.Property<string>("NameProvince")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("NameProvince");

                    b.ToTable("Province", (string)null);
                });

            modelBuilder.Entity("LoCoMPro_LV.Models.Record", b =>
                {
                    b.Property<string>("NameGenerator")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("RecordDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.Property<bool>("Hide")
                        .HasColumnType("bit");

                    b.Property<double>("Latitude")
                        .HasColumnType("float");

                    b.Property<double>("Longitude")
                        .HasColumnType("float");

                    b.Property<string>("NameProduct")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("NameStore")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<double?>("Price")
                        .IsRequired()
                        .HasColumnType("float");

                    b.HasKey("NameGenerator", "RecordDate");

                    b.HasIndex("NameProduct");

                    b.HasIndex("NameStore", "Latitude", "Longitude");

                    b.ToTable("Record", (string)null);
                });

            modelBuilder.Entity("LoCoMPro_LV.Models.Report", b =>
                {
                    b.Property<string>("NameGenerator")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("RecordDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("NameReporter")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("ReportDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Comment")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.HasKey("NameGenerator", "RecordDate", "NameReporter", "ReportDate");

                    b.HasIndex("NameReporter");

                    b.ToTable("Reports", (string)null);
                });

            modelBuilder.Entity("LoCoMPro_LV.Models.Store", b =>
                {
                    b.Property<string>("NameStore")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<double>("Latitude")
                        .HasColumnType("float");

                    b.Property<double>("Longitude")
                        .HasColumnType("float");

                    b.Property<string>("NameCanton")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("NameProvince")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("NameStore", "Latitude", "Longitude");

                    b.HasIndex("NameProvince", "NameCanton");

                    b.ToTable("Store", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("LoCoMPro_LV.Models.Anomalie", b =>
                {
                    b.HasOne("LoCoMPro_LV.Models.Record", "Record")
                        .WithMany("Anomalies")
                        .HasForeignKey("NameGenerator", "RecordDate")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Record");
                });

            modelBuilder.Entity("LoCoMPro_LV.Models.ApplicationUser", b =>
                {
                    b.HasOne("LoCoMPro_LV.Models.Canton", "Canton")
                        .WithMany("ApplicationUser")
                        .HasForeignKey("NameProvince", "NameCanton");

                    b.Navigation("Canton");
                });

            modelBuilder.Entity("LoCoMPro_LV.Models.Associated", b =>
                {
                    b.HasOne("LoCoMPro_LV.Models.Category", "Category")
                        .WithMany("Associated")
                        .HasForeignKey("NameCategory")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LoCoMPro_LV.Models.Product", "Product")
                        .WithMany("Associated")
                        .HasForeignKey("NameProduct")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("LoCoMPro_LV.Models.Canton", b =>
                {
                    b.HasOne("LoCoMPro_LV.Models.Province", "Province")
                        .WithMany("Cantons")
                        .HasForeignKey("NameProvince")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Province");
                });

            modelBuilder.Entity("LoCoMPro_LV.Models.Category", b =>
                {
                    b.HasOne("LoCoMPro_LV.Models.Category", "TopCategory")
                        .WithMany("Categories")
                        .HasForeignKey("NameTopCategory");

                    b.Navigation("TopCategory");
                });

            modelBuilder.Entity("LoCoMPro_LV.Models.Evaluate", b =>
                {
                    b.HasOne("LoCoMPro_LV.Models.GeneratorUser", "GeneratorUser")
                        .WithMany("Valorations")
                        .HasForeignKey("NameEvaluator")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LoCoMPro_LV.Models.Record", "Record")
                        .WithMany("Valorations")
                        .HasForeignKey("NameGenerator", "RecordDate")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GeneratorUser");

                    b.Navigation("Record");
                });

            modelBuilder.Entity("LoCoMPro_LV.Models.GeneratorUser", b =>
                {
                    b.HasOne("LoCoMPro_LV.Models.ApplicationUser", "ApplicationUser")
                        .WithMany("GeneratorUser")
                        .HasForeignKey("UserName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ApplicationUser");
                });

            modelBuilder.Entity("LoCoMPro_LV.Models.ModeratorUser", b =>
                {
                    b.HasOne("LoCoMPro_LV.Models.ApplicationUser", "ApplicationUser")
                        .WithMany("ModeratorUsers")
                        .HasForeignKey("UserName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ApplicationUser");
                });

            modelBuilder.Entity("LoCoMPro_LV.Models.Record", b =>
                {
                    b.HasOne("LoCoMPro_LV.Models.GeneratorUser", "GeneratorUser")
                        .WithMany("Record")
                        .HasForeignKey("NameGenerator")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LoCoMPro_LV.Models.Product", "Product")
                        .WithMany("Record")
                        .HasForeignKey("NameProduct")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LoCoMPro_LV.Models.Store", "Store")
                        .WithMany("Record")
                        .HasForeignKey("NameStore", "Latitude", "Longitude")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GeneratorUser");

                    b.Navigation("Product");

                    b.Navigation("Store");
                });

            modelBuilder.Entity("LoCoMPro_LV.Models.Report", b =>
                {
                    b.HasOne("LoCoMPro_LV.Models.GeneratorUser", "GeneratorUser")
                        .WithMany("Reports")
                        .HasForeignKey("NameReporter")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LoCoMPro_LV.Models.Record", "Record")
                        .WithMany("Reports")
                        .HasForeignKey("NameGenerator", "RecordDate")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GeneratorUser");

                    b.Navigation("Record");
                });

            modelBuilder.Entity("LoCoMPro_LV.Models.Store", b =>
                {
                    b.HasOne("LoCoMPro_LV.Models.Canton", "Canton")
                        .WithMany("Store")
                        .HasForeignKey("NameProvince", "NameCanton")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Canton");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("LoCoMPro_LV.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("LoCoMPro_LV.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LoCoMPro_LV.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("LoCoMPro_LV.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("LoCoMPro_LV.Models.ApplicationUser", b =>
                {
                    b.Navigation("GeneratorUser");

                    b.Navigation("ModeratorUsers");
                });

            modelBuilder.Entity("LoCoMPro_LV.Models.Canton", b =>
                {
                    b.Navigation("ApplicationUser");

                    b.Navigation("Store");
                });

            modelBuilder.Entity("LoCoMPro_LV.Models.Category", b =>
                {
                    b.Navigation("Associated");

                    b.Navigation("Categories");
                });

            modelBuilder.Entity("LoCoMPro_LV.Models.GeneratorUser", b =>
                {
                    b.Navigation("Record");

                    b.Navigation("Reports");

                    b.Navigation("Valorations");
                });

            modelBuilder.Entity("LoCoMPro_LV.Models.Product", b =>
                {
                    b.Navigation("Associated");

                    b.Navigation("Record");
                });

            modelBuilder.Entity("LoCoMPro_LV.Models.Province", b =>
                {
                    b.Navigation("Cantons");
                });

            modelBuilder.Entity("LoCoMPro_LV.Models.Record", b =>
                {
                    b.Navigation("Anomalies");

                    b.Navigation("Reports");

                    b.Navigation("Valorations");
                });

            modelBuilder.Entity("LoCoMPro_LV.Models.Store", b =>
                {
                    b.Navigation("Record");
                });
#pragma warning restore 612, 618
        }
    }
}
