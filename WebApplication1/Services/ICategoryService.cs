using Shop.ProductAPI.Models;
using Shop.ProductAPI.Models.DTOs;
using System.Linq.Expressions;

namespace Shop.ProductAPI.Services
{
    public interface ICategoryService : IService<CategoryDTO, Category>
    {
        Task<IEnumerable<CategoryProductDTO>> GetCategoriesProducts();
    }
}
