using Microsoft.AspNetCore.Http;
using Domain.Models.ViewModels;

namespace Application.Services
{
    public interface IEventService
    {
        IEnumerable<EventViewModel> GetAllEvents();
        EventViewModel GetEventById(long id);
        void AddEvent(EventViewModel eventViewModel);
        void EditEvent(SaveEventViewModel eventViewModel);
        void DeleteEventOccurrences(long eventId, bool isSeries);
        int GetEventPagesCount(IQueryCollection query);
        IEnumerable<EventViewModel> GetEventsPage(IQueryCollection query);
        List<DayOptionViewModel> GetWeeklyRecurrenceOptions();
        List<DayOptionViewModel> GetMonthlyRecurrenceOptions();
    }
}
