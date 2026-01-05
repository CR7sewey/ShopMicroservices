using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Shop.DiscountAPI.Context;
using Shop.DiscountAPI.Models;

namespace Shop.DiscountAPI.Repositories
{
    public class CouponRepository : ICouponRepository
    {

        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CouponRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _context = applicationDbContext;
            _mapper = mapper;
        }

        public Task<CouponDTO> CreateCoupon(CouponDTO couponDTO)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteCouponByProductName(string couponName)
        {
            var coupon = await _context.Coupons.FirstOrDefaultAsync(c => c.CouponCode == couponName);
            if (coupon is not null)
            {
                _context.Coupons.Remove(coupon);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<CouponDTO> GetCouponByProductName(string couponName)
        {
            if (string.IsNullOrEmpty(couponName))
            {
                return null;
            }

            CouponDTO? coupon = await _context.Coupons
                .Where(c => c.CouponCode == couponName)
                .Select(x => _mapper.Map<CouponDTO>(x)).FirstOrDefaultAsync();
                
            return coupon;
        }

        public async Task<bool> UpdateCoupon(CouponDTO couponDTO)
        {
            Coupon coupon = _mapper.Map<Coupon>(couponDTO);
            if (await _context.Coupons.AnyAsync(c => c.Id == coupon.Id))
            {
                _context.Coupons.Entry(coupon).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;

        }
    }
}
