using Task0.Models.Entities;

namespace Task0.Repositories
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
