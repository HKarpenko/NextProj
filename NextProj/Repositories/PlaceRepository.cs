using NextProj.Data;
using NextProj.Models.Entities;

namespace NextProj.Repositories
{
    public class PlaceRepository : BaseRepository, IPlaceRepository
    {
        private readonly AppDbContext _context;

        public PlaceRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<Place> GetAll()
        {
            return _context.Places.ToList();
        }
    }
}
