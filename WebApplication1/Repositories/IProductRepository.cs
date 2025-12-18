using Shop.ProductAPI.Models;
using System.Linq.Expressions;

namespace Shop.ProductAPI.Repositories
{
    public interface IProductRepository
    {
        Task<Product> GetById(Guid id);
        Task<IEnumerable<Product>> GetAll(Expression<Func<Product, bool>>? predicate);
        Task<Product> Create(Product category);
        Task<Product> Update(Product category);
        Task<Product> Delete(Guid id);
    }
}
