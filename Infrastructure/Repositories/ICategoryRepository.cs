using Domain.Models.Entities;

namespace Infrastructure.Repositories
{
    public interface ICategoryRepository : IBaseRepository
    {
        IEnumerable<Category> GetAll();
    }
}