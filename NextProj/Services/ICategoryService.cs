using NextProj.Models.Entities;

namespace NextProj.Services
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetAllCategories();
    }
}