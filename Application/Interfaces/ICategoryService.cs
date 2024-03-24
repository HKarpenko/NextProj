using Domain.Models.Entities;

namespace Application.Interfaces
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetAllCategories();
    }
}