using Microsoft.EntityFrameworkCore;
using Shop.ProductAPI.Context;
using Shop.ProductAPI.Models;
using Shop.ProductAPI.Models.DTOs;
using System.Linq.Expressions;

namespace Shop.ProductAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext applicationDbContext;

        public ProductRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public async Task<Product> Create(Product product)
        {
            if (product is null)
            {
                throw new ArgumentNullException(nameof(product));
            }
            await applicationDbContext.Products.AddAsync(product);
            return product;
        }

        public async Task<Product> Delete(Guid id)
        {
            if (id == null || id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }
            var prodExists = applicationDbContext.Products.Find(id);
            if (prodExists != null)
            {
                applicationDbContext.Products.Remove(prodExists);
                return prodExists;
            }
            throw new ArgumentNullException("Prod does not exist...");
        }

        public async Task<IEnumerable<Product>> GetAll(Expression<Func<Product, bool>>? predicate)
        {
            IQueryable<Product> data = applicationDbContext.Products.Include(p => p.Category).AsQueryable();

            if (predicate is not null)
            {
                data = data.Where(predicate);
            }

            return await data.ToListAsync();
        }

        public async Task<Product> GetById(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var product = await applicationDbContext.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
            return product;
        }

        public async Task<Product> Update(Product product)
        {
            var productExists = applicationDbContext.Products.AsNoTracking().Any(product  => product.Id == product.Id);
            if (productExists)
            {
                applicationDbContext.Products.Entry(product).State = EntityState.Modified;
                return product;
            }
            throw new InvalidOperationException("Product not found");
        }
    }
}
