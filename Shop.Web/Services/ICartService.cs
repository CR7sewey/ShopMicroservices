using Shop.Web.Models;

namespace Shop.Web.Services
{
    public interface ICartService
    {
        Task<CartViewModel> GetCartByUserId(Guid userId, string token);
        Task<CartViewModel> AddToCart(CartViewModel cartViewModel, string token);
        Task<CartViewModel> UpdateCart(CartViewModel cartViewModel, string token);

        Task<bool> RemoveFromCart(Guid userId, Guid productId, string token);
        Task<bool> ApplyCoupon(CartViewModel cartViewModel, Guid userId, string token);
        Task<bool> RemoveCoupon(Guid userId, string token);
        Task<CheckoutViewModel> CheckoutCompleted(CheckoutViewModel checkoutViewModel, string token);
    
        }
}
