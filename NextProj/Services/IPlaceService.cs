using NextProj.Models.Entities;

namespace NextProj.Services
{
    public interface IPlaceService
    {
        IEnumerable<Place> GetAllPlaces();
    }
}