using Domain.Models.Entities;
using Application.Interfaces;
using Infrastructure.Repositories.Interfaces;

namespace Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return _categoryRepository.GetAll();
        }
    }
}
