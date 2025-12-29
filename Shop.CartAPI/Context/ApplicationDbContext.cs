using Microsoft.EntityFrameworkCore;
using Shop.CartAPI.Models;

namespace Shop.CartAPI.Context
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        

        // Tables/Entites mappign definition - DbSet

        public DbSet<Product>? Products { get; set; }
        public DbSet<CartHeader>? CartHeaders { get; set; }

        public DbSet<CartItem>? CartItems { get; set; }


        // override generic convetions to create tables - FluentApi instead of DataAnnotations!
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Entity
            modelBuilder.Entity<Product>().HasKey(p => p.Id);
            modelBuilder.Entity<CartItem>().HasKey(ci => ci.Id);
            modelBuilder.Entity<CartHeader>().HasKey(ch => ch.Id);


            // properties
            modelBuilder.Entity<Product>().Property(p=> p.Name).HasMaxLength(100).IsRequired();
            modelBuilder.Entity<Product>().Property(p => p.Description).HasMaxLength(255).IsRequired();
            modelBuilder.Entity<Product>().Property(p => p.ImageUrl).HasMaxLength(1024).IsRequired();
            modelBuilder.Entity<Product>().Property(p => p.Price).HasPrecision(14, 2);


            // Relationship
            modelBuilder.Entity<CartItem>().HasOne(p => p.Product).WithMany(p => p.CartItems).HasForeignKey(p => p.ProductId);
            modelBuilder.Entity<CartItem>().HasOne(c => c.CartHeader).WithMany(c => c.CartItems).HasForeignKey(c => c.CartHeaderId);


            // Seed Data

            
            var p1 = new Product
            {
                Id = Guid.Parse("199184ab-3630-4f3c-8232-44a2bf9ac5b5"),
                Name = "The persue of hapiness",
                Price = 9.99m,
                Description = "Just a book",
                Stock = 111,
                ImageUrl = "",
            };
            var p2 = new Product
                {
                    Id = Guid.Parse("1f13c916-dd5d-4fe9-808d-c662917d6c5b"),
                    Name = "Seiko 1",
                    Price = 999m,
                    Description = "Just a watch",
                    Stock = 111,
                    ImageUrl = "",
                };


            var cartHeaderId = Guid.NewGuid();
            var cartHeader = new CartHeader
            {
                Id = cartHeaderId,
                UserId = Guid.Parse("01c3d0c8-3e3c-421c-b19d-c53d0bc751e5"),
                CouponCode = "DISC123",
            };

            modelBuilder.Entity<Product>().HasData(p1, p2);
            modelBuilder.Entity<CartHeader>().HasData(cartHeader);


            var cartItem = new CartItem
            {
                Id = Guid.NewGuid(),
                CartHeaderId = cartHeaderId,
                ProductId = Guid.Parse("199184ab-3630-4f3c-8232-44a2bf9ac5b5"),
            };

            modelBuilder.Entity<CartItem>().HasData(cartItem);

        }

    }
}
