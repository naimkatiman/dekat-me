using DekatMe.Api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DekatMe.Api.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Business> Businesses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<BusinessHour> BusinessHours { get; set; }
        public DbSet<BusinessImage> BusinessImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<Business>()
                .HasOne(b => b.Category)
                .WithMany(c => c.Businesses)
                .HasForeignKey(b => b.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Business)
                .WithMany(b => b.Reviews)
                .HasForeignKey(r => r.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BusinessHour>()
                .HasOne(bh => bh.Business)
                .WithMany(b => b.Hours)
                .HasForeignKey(bh => bh.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BusinessImage>()
                .HasOne(bi => bi.Business)
                .WithMany(b => b.Images)
                .HasForeignKey(bi => bi.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
