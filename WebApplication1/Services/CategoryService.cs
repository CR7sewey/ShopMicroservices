using AutoMapper;
using Shop.ProductAPI.Models;
using Shop.ProductAPI.Models.DTOs;
using Shop.ProductAPI.Repositories;
using System.Linq.Expressions;

namespace Shop.ProductAPI.Services
{
    public class CategoryService : IService<CategoryDTO, Category>
    {
        private readonly IMapper _mapper;
        private readonly  ICategoryRepository categoryRepository;

        public CategoryService(IMapper mapper, IUnitOfWork unitOfWork) {
            _mapper = mapper;
            categoryRepository = unitOfWork.CategoryRepository;
        }

        public async Task<IEnumerable<CategoryDTO>> GetAll(Expression<Func<Category, bool>>? predicate)
        {
            var categories = await categoryRepository.GetAll(predicate);
            var categoriesDTO = new List<CategoryDTO>();
            if (categories.Any())
            {
                foreach (var item in categories)
                {
                    categoriesDTO.Add(
                        _mapper.Map<CategoryDTO>(item)
                        );
                }
            }
            return categoriesDTO;
        }
    }
}
