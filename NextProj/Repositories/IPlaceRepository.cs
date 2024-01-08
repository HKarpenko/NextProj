using NextProj.Models.Entities;

namespace NextProj.Repositories
{
    public interface IPlaceRepository
    {
        List<Place> GetAll();
    }
}