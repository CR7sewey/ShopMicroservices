using Microsoft.EntityFrameworkCore;
using Shop.ProductAPI.Context;
using Shop.ProductAPI.Models;
using System.Linq.Expressions;

namespace Shop.ProductAPI.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {

        private readonly ApplicationDbContext applicationDbContext;

        public CategoryRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public Task<Category> Create(Category category)
        {
            throw new NotImplementedException();
        }

        public Task<Category> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Category>> GetAll(Expression<Func<Category, bool>>? predicate)
        {
            IQueryable<Category> data = applicationDbContext.Categories.AsQueryable();

            if (predicate is not null)
            {
                data = data.Where(predicate);
            }

            return await data.ToListAsync();

        }

        public Task<Category> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Category>> GetCategoriesProducts()
        {
            throw new NotImplementedException();
        }

        public Task<Category> Update(Category category)
        {
            throw new NotImplementedException();
        }
    }
}
