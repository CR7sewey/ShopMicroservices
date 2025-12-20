using AutoMapper;
using Shop.ProductAPI.Models;
using Shop.ProductAPI.Models.DTOs;
using Shop.ProductAPI.Repositories;
using System;
using System.Linq.Expressions;

namespace Shop.ProductAPI.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IMapper _mapper;
        private readonly  ICategoryRepository categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IMapper mapper, IUnitOfWork unitOfWork) {
            _mapper = mapper;
            categoryRepository = unitOfWork.CategoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CategoryDTO> Create(CategoryDTO entity)
        {
            if (entity != null)
            {
                var cat = _mapper.Map<Category>(entity);
                var categoria = await categoryRepository.Create(cat);
                await _unitOfWork.Save();
                entity.Id = categoria.Id;

            }
            return entity;

        }

        public async Task<CategoryDTO> Delete(Guid id)
        {
            var categoria = await categoryRepository.Delete(id);
            await _unitOfWork.Save();
            var categoryDTO = _mapper.Map<CategoryDTO>(categoria);
            return categoryDTO;
        }

        public async Task<IEnumerable<CategoryDTO>> GetAll(Expression<Func<Category, bool>>? predicate)
        {
            var categories = await categoryRepository.GetAll(predicate);
            var categoriesDTO = new List<CategoryDTO>();
         /*   if (categories.Any())
            {
                foreach (var item in categories)
                {
                    categoriesDTO.Add(
                        _mapper.Map<CategoryDTO>(item)
                        );
                }
            }*/
            return _mapper.Map<IEnumerable<CategoryDTO>>(categories);
        }

        public async Task<CategoryDTO> GetById(Guid id)
        {
            var category = await categoryRepository.GetById(id);
            var categoryDTO = _mapper.Map<CategoryDTO>(category);
            return categoryDTO;
        }

        public async Task<IEnumerable<CategoryProductDTO>> GetCategoriesProducts()
        {
            var categories = await categoryRepository.GetCategoriesProducts();
            return _mapper.Map<IEnumerable<CategoryProductDTO>>(categories);
        }

        public async Task<CategoryDTO> Update(CategoryDTO entity)
        {
            var cat = _mapper.Map<Category>(entity);
            var categoria = await categoryRepository.Update(cat);
            await _unitOfWork.Save();
            return entity;

        }
    }
}
