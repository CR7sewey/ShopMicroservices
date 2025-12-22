using AutoMapper;

namespace Shop.ProductAPI.Models.DTOs
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Product, ProductDTO>()
                .ForMember(p => p.CategoryName, opt => opt.MapFrom(src => src.Category.Name)); // Bind CategoryName (ProductDTO) from Category.Name (Product) - override mapping
            CreateMap<ProductDTO, Product>();
            CreateMap<Category, CategoryProductDTO>();
        }
    }
}
