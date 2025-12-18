using Microsoft.EntityFrameworkCore;
using Shop.ProductAPI.Context;
using Shop.ProductAPI.Models;
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

        public Task<Product> Create(Product category)
        {
            throw new NotImplementedException();
        }

        public Task<Product> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Product>> GetAll(Expression<Func<Product, bool>>? predicate)
        {
            IQueryable<Product> data = applicationDbContext.Products.AsQueryable();

            if (predicate is not null)
            {
                data = data.Where(predicate);
            }

            return await data.ToListAsync();
        }

        public Task<Product> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Product> Update(Product category)
        {
            throw new NotImplementedException();
        }
    }
}
