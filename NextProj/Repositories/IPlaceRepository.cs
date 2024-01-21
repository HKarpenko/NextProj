using NextProj.Models.Entities;

namespace NextProj.Repositories
{
    public interface IPlaceRepository : IBaseRepository
    {
        List<Place> GetAll();
    }
}