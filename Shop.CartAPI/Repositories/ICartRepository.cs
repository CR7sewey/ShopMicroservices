using Shop.CartAPI.Models;
using Shop.CartAPI.Models.ViewModel;

namespace Shop.CartAPI.Repositories
{
    public interface ICartRepository
    {
        Task<Cart> GetCartByUserId(Guid userId);
        Task<Cart> CreateUpdateCart(Cart cart);
        Task<bool> RemoveItemCart(Guid cartItemId);
        Task<bool> ClearCart(Guid userId);
        Task<bool> ApplyCoupon(Guid userId, string couponCode);
        Task<bool> RemoveCoupon(Guid userId);
    }
}
