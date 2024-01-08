using NextProj.Models.Entities;

namespace NextProj.Repositories
{
    public interface ICategoryRepository
    {
        List<Category> GetAll();
    }
}