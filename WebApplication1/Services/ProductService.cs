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
        private readonly IUnitOfWork _unitOfWork;


        public ProductService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            productRepository = unitOfWork.ProductRepository;
        }

        public async Task<ProductDTO> Create(ProductDTO entity)
        {
            if (entity != null)
            {
                var p = _mapper.Map<Product>(entity);
                var product = await productRepository.Create(p);
                await _unitOfWork.Save();
                entity.Id = product.Id;

            }
            return entity;
        }

        public async Task<ProductDTO> Delete(Guid id)
        {
            var product = await productRepository.Delete(id);
            await _unitOfWork.Save();
            var productDTO = _mapper.Map<ProductDTO>(product);
            return productDTO;
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

        public async Task<ProductDTO> GetById(Guid id)
        {
            var product = await productRepository.GetById(id);
            var productDTO = _mapper.Map<ProductDTO>(product);
            return productDTO;
        }


        public async Task<ProductDTO> Update(ProductDTO entity)
        {
            var prod = _mapper.Map<Product>(entity);
            var product = await productRepository.Update(prod);
            await _unitOfWork.Save();
            return entity;
        }
    }
}
