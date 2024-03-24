using Domain.Models.Entities;

namespace Infrastructure.Repositories.Interfaces
{
    public interface IPlaceRepository : IBaseRepository
    {
        IEnumerable<Place> GetAll();
    }
}