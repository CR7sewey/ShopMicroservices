using AutoMapper;
using Shop.ProductAPI.Models;
using Shop.ProductAPI.Models.DTOs;
using Shop.ProductAPI.Repositories;
using System.Linq.Expressions;

namespace Shop.ProductAPI.Services
{
    public class ProductService : IService<ProductDTO, Product>
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository productRepository;

        public ProductService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            productRepository = unitOfWork.ProductRepository;
        }

        public async Task<IEnumerable<ProductDTO>> GetAll(Expression<Func<Product, bool>>? predicate)
        {
            var products = await productRepository.GetAll(predicate);
            var productDTOs = new List<ProductDTO>();
            if (products.Any())
            {
                foreach (var item in products)
                {
                    productDTOs.Add(
                        _mapper.Map<ProductDTO>(item)
                        );
                }
            }
            return productDTOs;
        }
    }
}
