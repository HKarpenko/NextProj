using NextProj.Models.ViewModels;

namespace NextProj.Services
{
    public interface IEventService
    {
        List<EventViewModel> GetAllEvents();
        EventViewModel GetEventById(long id);
        void AddEvent(EventViewModel eventViewModel);
        void EditEvent(SaveEventViewModel eventViewModel);
        void DeleteEventOccurrences(long eventId, bool isSeries);
    }
}
