using Domain.Models.Entities;

namespace Infrastructure.Repositories.Interfaces
{
    public interface ICategoryRepository : IBaseRepository
    {
        IEnumerable<Category> GetAll();
    }
}