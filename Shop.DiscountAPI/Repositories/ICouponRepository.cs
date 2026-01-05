using Shop.DiscountAPI.Models;

namespace Shop.DiscountAPI.Repositories
{
    public interface ICouponRepository
    {

        Task<CouponDTO> CreateCoupon(CouponDTO couponDTO);
        Task<CouponDTO> GetCouponByProductName(string couponName);
        Task<bool> UpdateCoupon(CouponDTO couponDTO);
        Task<bool> DeleteCouponByProductName(string couponName);

    }
}
