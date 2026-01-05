using Microsoft.EntityFrameworkCore;
using Shop.DiscountAPI.Models;

namespace Shop.DiscountAPI.Context
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Coupon> Coupons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 
            modelBuilder.Entity<Coupon>().HasKey(c => c.Id);

            // Properties
            modelBuilder.Entity<Coupon>().Property(c => c.CouponCode).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Coupon>().Property(c => c.DiscountAmount).HasPrecision(14, 2);

            // seed data
            modelBuilder.Entity<Coupon>().HasData(
                new Coupon
                {
                    Id = Guid.NewGuid(),
                    CouponCode = "WELCOME10",
                    DiscountAmount = 10.00m,
                    CreatedAt = DateTime.UtcNow,
                    ExpiryDate = DateTime.UtcNow.AddMonths(1)
                },
                new Coupon
                {
                    Id = Guid.NewGuid(),
                    CouponCode = "SUMMER15",
                    DiscountAmount = 15.00m,
                    CreatedAt = DateTime.UtcNow,
                    ExpiryDate = DateTime.UtcNow.AddMonths(2)
                }
            );
        }
    }
}
