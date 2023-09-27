﻿using LoCoMPro_LV.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LoCoMPro_LV.Data

{
    public class LoComproContext : IdentityDbContext<ApplicationUser>
    {
        public LoComproContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Province> Provinces { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<GeneratorUser> GeneratorUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Province>().ToTable("Province");
            builder.Entity<Product>().ToTable("Product");
            builder.Entity<Store>().ToTable("Store");
            builder.Entity<GeneratorUser>().ToTable("GeneratorUser");

            builder.Entity<ApplicationUser>().HasKey(e => e.UserName);
            builder.Entity<ApplicationUser>().Property(e => e.UserName).IsRequired();

            builder.Entity<Canton>()
                .HasOne(c => c.Province)
                .WithMany(p => p.Cantons)
                .HasForeignKey(c => c.NameProvince);

            builder.Entity<Canton>()
                .HasKey(c => new { c.NameProvince, c.NameCanton});

            builder.Entity<Store>()
                .HasKey(s => new {s.NameStore});

            builder.Entity<Record>()
                .HasKey(r => new { r.NameRecord, r.RecordDate });

            builder.Entity<ApplicationUser>()
                .HasOne(c => c.Canton)
                .WithMany(e => e.ApplicationUser)
                .HasForeignKey(e => new { e.NameProvince, e.NameCanton });

            builder.Entity<GeneratorUser>()
                .HasOne(e => e.ApplicationUser)
                .WithMany(g => g.GeneratorUser)
                .HasForeignKey(g => new { g.UserName });

            builder.Entity<Store>()
                .HasOne(c => c.Canton)
                .WithMany(s => s.Store)
                .HasForeignKey(s => new { s.NameProvince, s.NameCanton });

            builder.Entity<ApplicationUser>().Ignore(e => e.Id);
            builder.Entity<ApplicationUser>().Ignore(e => e.PhoneNumber);
            builder.Entity<ApplicationUser>().Ignore(e => e.PhoneNumberConfirmed);
            builder.Entity<ApplicationUser>().Ignore(e => e.TwoFactorEnabled);
        }
    }
}