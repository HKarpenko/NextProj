using NextProj.Data;
using NextProj.Models.Entities;

namespace NextProj.Repositories
{
    public class PlaceRepository : IPlaceRepository
    {
        private readonly AppDbContext _context;

        public PlaceRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Place> GetAll()
        {
            return _context.Places.ToList();
        }
    }
}
