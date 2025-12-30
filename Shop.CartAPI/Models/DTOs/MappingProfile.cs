using AutoMapper;
using Shop.CartAPI.Models.ViewModel;

namespace Shop.CartAPI.Models.DTOs
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CartItem, CartItemDTO>().ReverseMap();
            CreateMap<CartHeader, CartHeaderDTO>().ReverseMap();
            CreateMap<CartDTO, Cart>().ReverseMap();
            CreateMap<ProductDTO, Product>().ReverseMap();
        }
    }
}
