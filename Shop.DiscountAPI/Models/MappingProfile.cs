using AutoMapper;

namespace Shop.DiscountAPI.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Coupon, CouponDTO>().ReverseMap();
        }
    }
}
