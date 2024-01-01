using NextProj.Data;
using NextProj.Models.Entities;

namespace NextProj.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly AppDbContext _context;

        public EventRepository(AppDbContext context)
        {
            _context = context;
        }

        public void AddEvent(Event eventModel)
        {
            _context.Events.Add(eventModel);
            _context.SaveChanges();
        }

        public void DeleteEvent(long id)
        {
            var eventModel = _context.Events.FirstOrDefault(e => e.Id == id);
            if (eventModel != null)
            {
                _context.Events.Remove(eventModel);
                _context.SaveChanges();
            }
        }

        public List<Event> GetAllEvents()
        {
            var list = _context.Events.ToList();

            return list;
        }

        public Event GetEventById(long id)
        {
            var eventModel = _context.Events.FirstOrDefault(e => e.Id == id);

            return eventModel;
        }

        public void SaveEvent(Event eventModel)
        {
            _context.Events.Update(eventModel);
            _context.SaveChanges();
        }
    }
}
