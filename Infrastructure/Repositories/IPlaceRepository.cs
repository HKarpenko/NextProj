using Domain.Models.Entities;

namespace Infrastructure.Repositories
{
    public interface IPlaceRepository : IBaseRepository
    {
        IEnumerable<Place> GetAll();
    }
}