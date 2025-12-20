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

        public async Task<Category> Create(Category category)
        {
            if (category == null)
            {
                throw new ArgumentNullException("Please insert a valid category");
            }

            await applicationDbContext.AddAsync(category);
            return category;

        }

        public async Task<Category> Delete(Guid id)
        {
            if (id == null || id == Guid.Empty)
            {
                throw new ArgumentNullException("Id cannot be null");
            }

            var catExists = applicationDbContext.Categories.Find(id);
            if (catExists is not null)
            {
                applicationDbContext.Categories.Remove(catExists);
                //await applicationDbContext.SaveChangesAsync();
                return catExists;
            }
            throw new ArgumentNullException(nameof(catExists));

        }

        public async Task<IEnumerable<Category>> GetAll(Expression<Func<Category, bool>>? predicate)
        {
            IQueryable<Category> data = applicationDbContext.Categories.AsQueryable();

            if (predicate is not null)
            {
                data = data.Where(predicate);
            }

            return await data.AsNoTracking().ToListAsync();

        }

        public async Task<Category> GetById(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var category = await applicationDbContext.Categories.FindAsync(id); // Where(c => c.Id == id).FirstOrDefault();
            return category;

        }

        public async Task<IEnumerable<Category>> GetCategoriesProducts()
        {
            IQueryable<Category> categories = applicationDbContext.Categories.Include(x => x.Products).AsQueryable().AsNoTracking();
            return await categories.ToListAsync();

        }

        public async Task<Category> Update(Category category)
        {

            // C#
            var tracked =await  applicationDbContext.Categories.AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == category.Id);
            //if (tracked != null) applicationDbContext.Entry(tracked.Entity).State = EntityState.Detached;
            if (tracked == null) throw new InvalidOperationException("Cat not found");

            applicationDbContext.Entry(category).State = EntityState.Modified;
            return category;

        }
    }
}
