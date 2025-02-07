﻿using Domain.Models.Entities;

namespace Infrastructure.Repositories.Interfaces
{
    public interface IEventRepository : IBaseRepository
    {
        IEnumerable<Event> GetAllEvents();
        Event GetEventById(long id);
        Event GetEventByOccurrenceId(long occurenceId);
        EventOccurrence GetEventOccurrenceById(long occurenceId);
        void AddEvent(Event eventModel);
        void SaveEvent(Event eventModel);
        void DeleteEvent(long eventId);
        void DeleteEventOccurrence(long occurrenceId);
        void DeleteEventDayRecurrence(long dayRecurrenceId);
    }
}
