using NextProj.Models.Entities;

namespace NextProj.Repositories
{
    public interface IEventRepository
    {
        List<Event> GetAllEvents();
        Event GetEventById(long id);
        void AddEvent(Event eventModel);
        void SaveEvent(Event eventModel);
        void DeleteEvent(long id);
    }
}
