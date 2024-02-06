using Domain.Models.Entities;

namespace Application.Services
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetAllCategories();
    }
}