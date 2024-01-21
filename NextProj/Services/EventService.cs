using NextProj.Models.Entities;
using NextProj.Models.Enums;
using NextProj.Models.ViewModels;
using NextProj.Repositories;

namespace NextProj.Services
{
    class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;

        private const int recurringDurationYears = 1;

        public EventService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public List<EventViewModel> GetAllEvents()
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
                RecurringType = ee.RecurringType
            }))
            .OrderBy(e => e.Time)
            .ToList();
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
                RecurringType = eventOccurrence.Event.RecurringType
            };
        }
        public void AddEvent(EventViewModel eventViewModel)
        {
            var occurrences = eventViewModel.RecurringType == null ? 
                new List<EventOccurrence> (){ new EventOccurrence() { Time = eventViewModel.Time } }
                : GenerateOccurrences(eventViewModel.Time, (RecurringType)eventViewModel.RecurringType);

            _eventRepository.AddEvent(CreateNewEvent(eventViewModel,occurrences));
            _eventRepository.SaveChanges();
        }

        private List<EventOccurrence> GenerateOccurrences(DateTime startDate, RecurringType recurringType)
        {
            var resultOccurrences = new List<EventOccurrence>();
            var currentTime = startDate;
            var endTime = startDate.AddYears(recurringDurationYears);

            while (currentTime <= endTime)
            {
                resultOccurrences.Add(new EventOccurrence() { Time = currentTime });

                currentTime += recurringType switch
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

        public void DeleteEventOccurrences(long occurenceId, bool isSeries)
        {
            if(isSeries)
            {
                var occurrence = _eventRepository.GetEventOccurrenceById(occurenceId);
                _eventRepository.DeleteEvent(occurrence.EventId);
            }
            else
            {
                _eventRepository.DeleteEventOccurrence(occurenceId);
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
                    var generatedOccurrences = GenerateOccurrences(eventViewModel.Time, (RecurringType)eventViewModel.RecurringType);
                    if (eventModel.RecurringType == null)
                    {
                        eventModel.Occurrences = eventModel.Occurrences.Concat(generatedOccurrences.Skip(1)).ToList();
                    }
                    else
                    {
                        eventModel.Occurrences.ToList().ForEach(oc => _eventRepository.DeleteEventOccurrence(oc.Id));
                        eventModel.Occurrences = generatedOccurrences;
                    }
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
                Occurrences = occurrences
            };
        }
    }
}
