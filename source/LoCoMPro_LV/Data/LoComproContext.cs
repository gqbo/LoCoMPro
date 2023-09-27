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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>().HasKey(e => e.UserName);
            builder.Entity<ApplicationUser>().Property(e => e.UserName).IsRequired();

            builder.Entity<Canton>()
                .HasOne(c => c.Province)
                .WithMany(p => p.Cantons)
                .HasForeignKey(c => c.NameProvince);

            builder.Entity<Canton>()
                .HasKey(c => new { c.NameProvince, c.NameCanton});

            builder.Entity<ApplicationUser>()
                .HasOne(c => c.Canton)
                .WithMany(e => e.ApplicationUser)
                .HasForeignKey(e => new { e.NameProvince, e.NameCanton });

            builder.Entity<Province>().ToTable("Province");

            builder.Entity<ApplicationUser>().Ignore(e => e.Id);
            builder.Entity<ApplicationUser>().Ignore(e => e.PhoneNumber);
            builder.Entity<ApplicationUser>().Ignore(e => e.PhoneNumberConfirmed);
            builder.Entity<ApplicationUser>().Ignore(e => e.TwoFactorEnabled);
        }
    }
}