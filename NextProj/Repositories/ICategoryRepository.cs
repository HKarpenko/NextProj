using NextProj.Models.Entities;

namespace NextProj.Repositories
{
    public interface ICategoryRepository : IBaseRepository
    {
        List<Category> GetAll();
    }
}