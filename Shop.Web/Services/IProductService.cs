using Shop.Web.Models;

namespace Shop.Web.Services
{
    public interface IProductService
    {

        Task<IEnumerable<ProductViewModel>> GetProducts();
        Task<ProductViewModel> GetProduct(Guid id);
        Task<ProductViewModel> CreateProduct(ProductViewModel productVM, string token);
        Task<ProductViewModel> UpdateProduct(Guid id, ProductViewModel productVM, string token);
        Task<bool> DeleteProduct(Guid id, string token);


    }
}
