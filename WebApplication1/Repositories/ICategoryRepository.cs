using Shop.ProductAPI.Models;
using System.Linq.Expressions;

namespace Shop.ProductAPI.Repositories
{
    public interface ICategoryRepository
    {
        Task<Category> GetById(Guid id);
        Task<IEnumerable<Category>> GetAll(Expression<Func<Category, bool>>? predicate);
        Task<IEnumerable<Category>> GetCategoriesProducts();

        Task<Category> Create(Category category);
        Task<Category> Update(Category category);
        Task<Category> Delete(Guid id);

    }
}
