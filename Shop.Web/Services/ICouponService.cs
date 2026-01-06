using Shop.Web.Models;

namespace Shop.Web.Services
{
    public interface ICouponService
    {
        Task<CouponViewModel> GetCoupon(string couponCode, string token);

    }
}
