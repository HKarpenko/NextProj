using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;

namespace Infrastructure.Repositories
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

        public void DeleteEventDayRecurrence(long dayRecurrenceId)
        {
            var dayRecurrence = _context.DayRecurrences.FirstOrDefault(rd => rd.Id == dayRecurrenceId);
            if (dayRecurrence != null)
            {
                _context.DayRecurrences.Remove(dayRecurrence);
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
                .Include(eo => eo.Event)
                    .ThenInclude(e => e.DayRecurrences)
                        .ThenInclude(dr => dr.DayPositions)
                .FirstOrDefault(e => e.Id == occurenceId);

            return eventOccurrence;
        }

        public void SaveEvent(Event eventModel)
        {
            _context.Events.Update(eventModel);
        }
    }
}
