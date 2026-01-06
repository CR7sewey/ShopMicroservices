using AutoMapper;
using Shop.CartAPI.Models.DTOs;
using Shop.CartAPI.Models.ViewModel;
using Shop.CartAPI.Repositories;

namespace Shop.CartAPI.Services
{
    public class CartService
    {
        private readonly ILogger<CartService> logger;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;

        public CartService(ILogger<CartService> logger, IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

        public async Task<bool> ClearCartAsync(Guid userId)
        {
            try
            {
                var cleared = await unitOfWork.CartRepository.ClearCart(userId);
                if (cleared)
                {
                    await unitOfWork.Save();
                }
                return cleared;

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error clearing cart for user {UserId}", userId);
                return false;
            }
        }

        public async Task<CartDTO?> GetCartByUserIdAsync(Guid userId)
        {
            try
            {
                var cart = await unitOfWork.CartRepository.GetCartByUserId(userId);
                return cart is null ? null : mapper.Map<CartDTO>(cart);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving cart for user {UserId}", userId);
                return null;
            }
        }

        public async Task<CartDTO> UpdateCart(CartDTO cartDto)
        {
            try
            {
                var cart = mapper.Map<Cart>(cartDto);
                var updatedCart = await unitOfWork.CartRepository.CreateUpdateCart(cart);
                await unitOfWork.Save();
                return mapper.Map<CartDTO>(updatedCart);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error updating cart for user {UserId}", cartDto.CartHeader.UserId);
                throw;
            }
        }

        public async Task<bool> RemoveItemCartAsync(Guid cartItemId)
        {
            try
            {
                var removed = await unitOfWork.CartRepository.RemoveItemCart(cartItemId);
                if (removed)
                {
                    await unitOfWork.Save();
                }
                return removed;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error removing cart item {CartItemId}", cartItemId);
                return false;
            }
        }

        public async Task<bool> ApplyCouponAsync(Guid userId, CartDTO cartDTO)
        {
            try
            {
                var applied = await unitOfWork.CartRepository.ApplyCoupon(userId, cartDTO.CartHeader.CouponCode);
                if (applied)
                {
                    await unitOfWork.Save();
                }
                return applied;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error applying coupon for user {UserId}", userId);
                return false;
            }
        }

        public async Task<bool> RemoveCouponAsync(Guid userId)
        {
            try
            {
                var removed = await unitOfWork.CartRepository.RemoveCoupon(userId);
                if (removed)
                {
                    await unitOfWork.Save();
                }
                return removed;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error removing coupon for user {UserId}", userId);
                return false;
            }
        }

    }
}
