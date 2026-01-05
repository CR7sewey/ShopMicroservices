using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop.DiscountAPI.Models;
using Shop.DiscountAPI.Repositories;

namespace Shop.DiscountAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponController : ControllerBase
    {
        private readonly ILogger<CouponController> _logger;
        private readonly ICouponRepository _couponRepository;

        public CouponController(ILogger<CouponController> logger, ICouponRepository couponRepository)
        {
            _logger = logger;
            _couponRepository = couponRepository;
        }

        [Authorize]
        [HttpGet("{couponCode}", Name = "GetCoupon")]
        public async Task<IActionResult> GetCoupon(string couponCode)
        {
            var coupon = await _couponRepository.GetCouponByProductName(couponCode);
            if (coupon == null)
            {
                _logger.LogWarning("Coupon not found for coupon: {Coupon Code}", couponCode);
                return NotFound();
            }
            _logger.LogInformation("Coupon retrieved for coupon: {Coupon Code}, Amount: {Amount}", couponCode, coupon.DiscountAmount);
            return Ok(coupon);
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateCoupon([FromBody] CouponDTO coupon)
        {
            var updateResult = await _couponRepository.UpdateCoupon(coupon);
            if (!updateResult)
            {
                _logger.LogWarning("Failed to update coupon: {Coupon Code}", coupon.CouponCode);
                return BadRequest("Failed to update coupon.");
            }
            _logger.LogInformation("Coupon updated successfully: {Coupon Code}", coupon.CouponCode);
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{couponCode}")]
        public async Task<IActionResult> DeleteCoupon(string couponCode)
        {
            var deleteResult = await _couponRepository.DeleteCouponByProductName(couponCode);
            if (!deleteResult)
            {
                _logger.LogWarning("Failed to delete coupon: {Coupon Code}", couponCode);
                return BadRequest("Failed to delete coupon.");
            }
            _logger.LogInformation("Coupon deleted successfully: {Coupon Code}", couponCode);
            return NoContent();
        }
    }
}
