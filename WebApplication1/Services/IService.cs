using Shop.ProductAPI.Models;
using System.Linq.Expressions;

namespace Shop.ProductAPI.Services
{
    public interface IService<T,R> where T : class where R : class
    {
        Task<IEnumerable<T>> GetAll(Expression<Func<R, bool>>? predicate);
        Task<T> GetById(Guid id);
        Task<T> Create(T entity);
        Task<T> Update(T entity);
        Task<T> Delete(Guid id);
    }
}
