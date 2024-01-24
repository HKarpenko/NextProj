using Microsoft.EntityFrameworkCore;
using NextProj.Data;
using NextProj.Models.Entities;

namespace NextProj.Repositories
{
    public class EventRepository : BaseRepository, IEventRepository
    {
        private readonly AppDbContext _context;

        public EventRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public void AddEvent(Event eventModel)
        {
            _context.Events.Add(eventModel);
        }

        public void DeleteEvent(long eventId)
        {
            var eventModel = _context.Events.FirstOrDefault(e => e.Id == eventId);
            if (eventModel != null)
            {
                _context.Events.Remove(eventModel);
            }
        }

        public void DeleteEventOccurrence(long occurrenceId)
        {
            var eventOccurrence = _context.EventsOccurrences.FirstOrDefault(e => e.Id == occurrenceId);
            if (eventOccurrence != null)
            {
                _context.EventsOccurrences.Remove(eventOccurrence);
            }
        }

        public IEnumerable<Event> GetAllEvents()
        {
            var events = _context.Events
                .Include(p => p.Place)
                .Include(c => c.Category)
                .Include(o => o.Occurrences);

            return events;
        }

        public Event GetEventById(long id)
        {
            var eventModel = _context.Events
                .Include(p => p.Place)
                .Include(c => c.Category)
                .Include(o => o.Occurrences)
                .FirstOrDefault(e => e.Id == id);

            return eventModel;
        }

        public Event GetEventByOccurrenceId(long occurenceId)
        {
            var occurrence = _context.EventsOccurrences.FirstOrDefault(oc => oc.Id == occurenceId);

            return GetEventById(occurrence.EventId);
        }

        public EventOccurrence GetEventOccurrenceById(long occurenceId)
        {
            var eventOccurrence = _context.EventsOccurrences
                .Include(eo => eo.Event)
                    .ThenInclude(e => e.Category)
                .Include(eo => eo.Event)
                    .ThenInclude(e => e.Place)
                .FirstOrDefault(e => e.Id == occurenceId);

            return eventOccurrence;
        }

        public void SaveEvent(Event eventModel)
        {
            _context.Events.Update(eventModel);
        }
    }
}
