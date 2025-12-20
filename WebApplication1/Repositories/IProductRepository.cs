using Shop.ProductAPI.Models;
using System.Linq.Expressions;

namespace Shop.ProductAPI.Repositories
{
    public interface IProductRepository
    {
        Task<Product> GetById(Guid id);
        Task<IEnumerable<Product>> GetAll(Expression<Func<Product, bool>>? predicate);
        Task<Product> Create(Product product);
        Task<Product> Update(Product product);
        Task<Product> Delete(Guid id);
    }
}
