using Microsoft.Extensions.Logging;
using NextProj.Models.Entities;
using NextProj.Models.Enums;
using NextProj.Models.ViewModels;
using NextProj.Repositories;

namespace NextProj.Services
{
    class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;

        private const int pageSize = 10;

        public EventService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public IEnumerable<EventViewModel> GetAllEvents()
        {
            var eventEntities = _eventRepository.GetAllEvents();

            return eventEntities.SelectMany(ee => ee.Occurrences.Select(oc => new EventViewModel
            {
                Id = oc.Id,
                Name = ee.Name,
                Images = ee.Images,
                Description = ee.Description,
                CategoryId = ee.CategoryId,
                Category = ee.Category.Name,
                PlaceId = ee.PlaceId,
                Place = ee.Place.DisplayName,
                AdditionalInfo = ee.AdditionalInfo,
                Time = oc.Time,
                RecurringType = ee.RecurringType,
                RecurringUntil = ee.RecurringUntil
            }))
            .OrderBy(e => e.Time);
        }

        public EventViewModel GetEventById(long id)
        {
            var eventOccurrence = _eventRepository.GetEventOccurrenceById(id);

            return new EventViewModel
            {
                Id = eventOccurrence.Id,
                Name = eventOccurrence.Event.Name,
                Images = eventOccurrence.Event.Images,
                Description = eventOccurrence.Event.Description,
                CategoryId = eventOccurrence.Event.CategoryId,
                Category = eventOccurrence.Event.Category.Name,
                PlaceId = eventOccurrence.Event.PlaceId,
                Place = eventOccurrence.Event.Place.DisplayName,
                AdditionalInfo = eventOccurrence.Event.AdditionalInfo,
                Time = eventOccurrence.Time,
                RecurringType = eventOccurrence.Event.RecurringType,
                RecurringUntil = eventOccurrence.Event.RecurringUntil
            };
        }
        public void AddEvent(EventViewModel eventViewModel)
        {
            var occurrences = eventViewModel.RecurringType == null ? 
                new List<EventOccurrence> (){ new EventOccurrence() { Time = eventViewModel.Time } }
                : GenerateOccurrences(eventViewModel);

            _eventRepository.AddEvent(CreateNewEvent(eventViewModel,occurrences));
            _eventRepository.SaveChanges();
        }

        public void DeleteEventOccurrences(long occurrenceId, bool isSeries)
        {
            if(isSeries)
            {
                var occurrence = _eventRepository.GetEventOccurrenceById(occurrenceId);
                _eventRepository.DeleteEvent(occurrence.EventId);
            }
            else
            {
                _eventRepository.DeleteEventOccurrence(occurrenceId);
            }
            _eventRepository.SaveChanges();
        }

        public void EditEvent(SaveEventViewModel eventViewModel)
        {
            var eventModel = _eventRepository.GetEventByOccurrenceId(eventViewModel.Id);
            var occurrence = eventModel.Occurrences.FirstOrDefault(oc => oc.Id == eventViewModel.Id);
            if (eventViewModel.RecurringType == null)
            {
                occurrence.Time = eventViewModel.Time;
                if (eventModel.RecurringType != null && !eventViewModel.isSeries)
                {
                    eventModel.Occurrences = eventModel.Occurrences.Where(oc => oc.Id != occurrence.Id).ToList();
                    _eventRepository.AddEvent(CreateNewEvent(eventViewModel, new List<EventOccurrence>() { occurrence }));
                }
                else
                {
                    UpdateEvent(eventViewModel, eventModel);
                    if (eventViewModel.isSeries)
                    {
                        eventModel.Occurrences = eventModel.Occurrences.Where(oc => oc.Id == occurrence.Id).ToList();
                    }
                }
            }
            else
            {
                if (eventModel.RecurringType != null && !eventViewModel.isSeries)
                {
                    occurrence.Time = eventViewModel.Time;
                }
                else
                {
                    var generatedOccurrences = GenerateOccurrences(eventViewModel);
                    if (eventModel.RecurringType == null)
                    {
                        generatedOccurrences = eventModel.Occurrences.Concat(generatedOccurrences.Skip(1)).ToList();
                    }
                    else
                    {
                        eventModel.Occurrences.ToList().ForEach(oc => _eventRepository.DeleteEventOccurrence(oc.Id));
                    }
                    eventModel.Occurrences = generatedOccurrences;
                    UpdateEvent(eventViewModel, eventModel);
                }
            }

            _eventRepository.SaveEvent(eventModel);
            _eventRepository.SaveChanges();
        }

        private void UpdateEvent(EventViewModel eventViewModel, Event eventModel)
        {
            eventModel.Name = eventViewModel.Name;
            eventModel.Images = eventViewModel.Images;
            eventModel.Description = eventViewModel.Description;
            eventModel.CategoryId = eventViewModel.CategoryId;
            eventModel.PlaceId = eventViewModel.PlaceId;
            eventModel.RecurringType = eventViewModel.RecurringType;
            eventModel.RecurringUntil = eventModel.RecurringUntil;
            eventModel.AdditionalInfo = eventViewModel.AdditionalInfo;
        }

        private Event CreateNewEvent(EventViewModel eventViewModel, List<EventOccurrence> occurrences)
        {
            return new Event
            {
                Name = eventViewModel.Name,
                AdditionalInfo = eventViewModel.AdditionalInfo,
                CategoryId = eventViewModel.CategoryId,
                Description = eventViewModel.Description,
                Images = eventViewModel.Images,
                PlaceId = eventViewModel.PlaceId,
                RecurringType = eventViewModel.RecurringType,
                RecurringUntil = eventViewModel.RecurringUntil,
                Occurrences = occurrences
            };
        }

        private List<EventOccurrence> GenerateOccurrences(EventViewModel viewModel)
        {
            var resultOccurrences = new List<EventOccurrence>();
            var currentTime = viewModel.Time;
            var endTime = viewModel.RecurringUntil;

            while (currentTime <= endTime)
            {
                resultOccurrences.Add(new EventOccurrence() { Time = currentTime });

                currentTime += viewModel.RecurringType switch
                {
                    RecurringType.Daily => TimeSpan.FromDays(1),
                    RecurringType.Weekly => TimeSpan.FromDays(7),
                    RecurringType.Biweekly => TimeSpan.FromDays(14),
                    RecurringType.Monthly => (currentTime.AddMonths(1) - currentTime),
                    _ => throw new ArgumentException("Invalid recurring type")
                };
            }

            return resultOccurrences;
        }

        private IEnumerable<EventViewModel> GetFilteredEvents(IQueryCollection query)
        {
            var allEvents = GetAllEvents();
            if(query.Count == 0)
            {
                return allEvents;
            }
            string categoryFilter = query["category"];
            string placeFilter = query["place"];
            DateTime? timeFilter = string.IsNullOrEmpty(query["time"]) ? null : DateTime.Parse(query["time"]);

            return allEvents.Where(e => (string.IsNullOrEmpty(categoryFilter) || e.CategoryId.ToString() == categoryFilter)
                && (string.IsNullOrEmpty(placeFilter) || e.PlaceId.ToString() == placeFilter)
                && (timeFilter == null || e.Time > timeFilter));
        }

        public IEnumerable<EventViewModel> GetEventsPage(IQueryCollection query)
        {
            var filteredEvents = GetFilteredEvents(query);
            int page = string.IsNullOrEmpty(query["page"]) ? 1 : int.Parse(query["page"]);

            return filteredEvents.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public int GetEventPagesCount(IQueryCollection query)
        {
            var eventsCount = GetFilteredEvents(query).Count();
            return (int)Math.Ceiling(eventsCount / (double)pageSize);
        }
    }
}
