using Domain.Models.Entities;

namespace Application.Services
{
    public interface IPlaceService
    {
        IEnumerable<Place> GetAllPlaces();
    }
}