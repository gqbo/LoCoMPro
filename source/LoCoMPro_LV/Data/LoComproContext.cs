using LoCoMPro_LV.Models;
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
        public DbSet<Canton> Cantons { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<GeneratorUser> GeneratorUsers { get; set; }
        public DbSet<ModeratorUser> ModeratorUsers { get; set; }
        public DbSet<Record> Records { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Associated> Associated { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Evaluate> Valorations { get; set; }
        public DbSet<Evaluate> Lists { get; set; }
        public DbSet<Evaluate> Listed { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Province>().ToTable("Province");
            builder.Entity<Canton>().ToTable("Cantons");
            builder.Entity<Product>().ToTable("Product");
            builder.Entity<Store>().ToTable("Store");
            builder.Entity<GeneratorUser>().ToTable("GeneratorUser");
            builder.Entity<ModeratorUser>().ToTable("ModeratorUser");
            builder.Entity<Record>().ToTable("Record");
            builder.Entity<Category>().ToTable("Category");
            builder.Entity<Associated>().ToTable("Associated");
            builder.Entity<Report>().ToTable("Reports");
            builder.Entity<Evaluate>().ToTable("Valorations");
            builder.Entity<List>().ToTable("Lists");
            builder.Entity<Listed>().ToTable("Listed");

            builder.Entity<ApplicationUser>().HasKey(e => e.UserName);
            builder.Entity<ApplicationUser>().Property(e => e.UserName).IsRequired();

            builder.Entity<Canton>()
                .HasOne(c => c.Province)
                .WithMany(p => p.Cantons)
                .HasForeignKey(c => c.NameProvince);

            builder.Entity<Canton>()
                .HasKey(c => new { c.NameProvince, c.NameCanton});

            builder.Entity<Store>()
                .HasKey(s => new {s.NameStore, s.Latitude, s.Longitude});

            builder.Entity<Category>()
                .HasKey(ca => new {ca.NameCategory});

            builder.Entity<Record>()
                .HasKey(r => new { r.NameGenerator, r.RecordDate });
            
            builder.Entity<Associated>()
                .HasKey(a => new { a.NameProduct, a.NameCategory });

            builder.Entity<Report>()
                .HasKey(r => new { r.NameGenerator, r.RecordDate, r.NameReporter, r.ReportDate });

            builder.Entity<Evaluate>()
                .HasKey(r => new { r.NameGenerator, r.RecordDate, r.NameEvaluator });

            builder.Entity<List>()
                .HasKey(r => new { r.NameList, r.UserName});

            builder.Entity<Listed>()
                .HasKey(r => new { r.NameList, r.UserName, r.NameProduct });

            builder.Entity<ApplicationUser>()
                .HasOne(c => c.Canton)
                .WithMany(e => e.ApplicationUser)
                .HasForeignKey(e => new { e.NameProvince, e.NameCanton });

            builder.Entity<GeneratorUser>()
                .HasOne(e => e.ApplicationUser)
                .WithMany(g => g.GeneratorUser)
                .HasForeignKey(g => new { g.UserName });

            builder.Entity<ModeratorUser>()
                .HasOne(e => e.ApplicationUser)
                .WithMany(g => g.ModeratorUsers)
                .HasForeignKey(g => new { g.UserName });

            builder.Entity<Store>()
                .HasOne(c => c.Canton)
                .WithMany(s => s.Store)
                .HasForeignKey(s => new { s.NameProvince, s.NameCanton });

            builder.Entity<Record>()
                .HasOne(c => c.Store)
                .WithMany(r => r.Record)
                .HasForeignKey(r => new { r.NameStore, r.Latitude, r.Longitude });

            builder.Entity<Record>()
                .HasOne(c => c.GeneratorUser)
                .WithMany(r => r.Record)
                .HasForeignKey(r => new { r.NameGenerator });

            builder.Entity<Record>()
                .HasOne(c => c.Product)
                .WithMany(r => r.Record)
                .HasForeignKey(r => new { r.NameProduct });

            builder.Entity<Category>()
                .HasOne(ca => ca.TopCategory)
                .WithMany(ca => ca.Categories)
                .HasForeignKey(ca => new { ca.NameTopCategory});

            builder.Entity<Associated>()
                .HasOne(ca => ca.Product)
                .WithMany(a => a.Associated)
                .HasForeignKey(a => new { a.NameProduct});

            builder.Entity<Associated>()
                .HasOne(ca => ca.Category)
                .WithMany(a => a.Associated)
                .HasForeignKey(a => new { a.NameCategory});

            builder.Entity<Report>()
                .HasOne(r => r.Record)
                .WithMany(re => re.Reports)
                .HasForeignKey(re => new { re.NameGenerator, re.RecordDate });

            builder.Entity<Report>()
                .HasOne(c => c.GeneratorUser)
                .WithMany(r => r.Reports)
                .HasForeignKey(r => new { r.NameReporter });

            builder.Entity<Evaluate>()
                .HasOne(r => r.Record)
                .WithMany(re => re.Valorations)
                .HasForeignKey(re => new { re.NameGenerator, re.RecordDate });

            builder.Entity<Evaluate>()
                .HasOne(c => c.GeneratorUser)
                .WithMany(r => r.Valorations)
                .HasForeignKey(r => new { r.NameEvaluator });

            builder.Entity<Listed>()
                .HasOne(r => r.List)
                .WithMany(re => re.Listed)
                .HasForeignKey(re => new { re.NameList, re.UserName });

            builder.Entity<Listed>()
                .HasOne(r => r.Product)
                .WithMany(re => re.Listed)
                .HasForeignKey(re => new { re.NameProduct });

            builder.Entity<List>()
                .HasOne(r => r.User)
                .WithMany(re => re.Lists)
                .HasForeignKey(re => new { re.UserName });

            builder.Entity<ApplicationUser>().Ignore(e => e.Id);
            builder.Entity<ApplicationUser>().Ignore(e => e.PhoneNumber);
            builder.Entity<ApplicationUser>().Ignore(e => e.PhoneNumberConfirmed);
            builder.Entity<ApplicationUser>().Ignore(e => e.TwoFactorEnabled);
        }
    }
}