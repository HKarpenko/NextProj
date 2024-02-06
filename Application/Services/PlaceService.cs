using Domain.Models.Entities;
using Infrastructure.Repositories;

namespace Application.Services
{
    public class PlaceService : IPlaceService
    {
        private readonly IPlaceRepository _placeRepository;

        public PlaceService(IPlaceRepository placeRepository) 
        {
            _placeRepository = placeRepository;
        }

        public IEnumerable<Place> GetAllPlaces()
        {
            return _placeRepository.GetAll();
        }
    }
}
