using Microsoft.EntityFrameworkCore;
using Shop.ProductAPI.Models;

namespace Shop.ProductAPI.Context
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        

        // Tables/Entites mappign definition - DbSet

        public DbSet<Product>? Products { get; set; }
        public DbSet<Category>? Categories { get; set; }

        // override generic convetions to create tables - FluentApi instead of DataAnnotations!
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Entity
            modelBuilder.Entity<Product>().HasKey(p => p.Id);
            modelBuilder.Entity<Category>().HasKey(p => p.Id);


            // properties
            modelBuilder.Entity<Product>().Property(p=> p.Name).HasMaxLength(100).IsRequired();
            modelBuilder.Entity<Product>().Property(p => p.Description).HasMaxLength(255).IsRequired();
            modelBuilder.Entity<Product>().Property(p => p.ImageUrl).HasMaxLength(1024).IsRequired();
            modelBuilder.Entity<Product>().Property(p => p.Price).HasPrecision(14, 2);

            modelBuilder.Entity<Category>().Property(p => p.Name).HasMaxLength(100).IsRequired();



            // Relationship
            modelBuilder.Entity<Product>().HasOne(p => p.Category).WithMany(p => p.Products).HasForeignKey(p => p.CategoryId);


            // Seed Data
            var cat1 = new Category
            {
                Id = Guid.NewGuid(),
                Name = "Books"
            };
            var cat2 = new Category
            {
                Id = Guid.NewGuid(),
                Name = "Watches"
            };
                

            modelBuilder.Entity<Category>().HasData(
                cat1,
                cat2
                );

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "The persue of hapiness",
                    Price = 9.99m,
                    Description = "Just a book",
                    Stock = 111,
                    ImageUrl = "",
                    CategoryId = cat1.Id
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Seiko 1",
                    Price = 999m,
                    Description = "Just a watch",
                    Stock = 111,
                    ImageUrl = "",
                    CategoryId = cat2.Id
                }
                );
        }

    }
}
