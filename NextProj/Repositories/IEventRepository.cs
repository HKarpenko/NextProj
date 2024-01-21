using NextProj.Models.Entities;

namespace NextProj.Repositories
{
    public interface IEventRepository : IBaseRepository
    {
        List<Event> GetAllEvents();
        Event GetEventById(long id);
        Event GetEventByOccurrenceId(long occurenceId);
        EventOccurrence GetEventOccurrenceById(long occurenceId);
        void AddEvent(Event eventModel);
        void SaveEvent(Event eventModel);
        void DeleteEvent(long eventId);
        void DeleteEventOccurrence(long occurrenceId);
    }
}
