using Domain.Models.Entities;

namespace Application.Interfaces
{
    public interface IPlaceService
    {
        IEnumerable<Place> GetAllPlaces();
    }
}