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
        private const int amountOfDaysInWeek = 7;

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
                RecurringUntil = eventOccurrence.Event.RecurringUntil,
                RecurrenceDays = eventOccurrence.Event.DayRecurrences.Select(dr => 
                    new RecurrenceDayViewModel 
                    {
                        Day = dr.Day,
                        DayPositions = dr.DayPositions.Count == 0 ? null : dr.DayPositions.Select(dp => dp.DayPosition).ToList()
                    }
                ).ToList()
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
                var eventModel = _eventRepository.GetEventByOccurrenceId(occurrenceId);
                _eventRepository.DeleteEvent(eventModel.Id);
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

                    eventModel.Occurrences.ToList().ForEach(oc => _eventRepository.DeleteEventOccurrence(oc.Id));
                    eventModel.Occurrences = generatedOccurrences;

                    UpdateEvent(eventViewModel, eventModel);
                }
            }

            _eventRepository.SaveEvent(eventModel);
            _eventRepository.SaveChanges();
        }

        private void UpdateEvent(EventViewModel eventViewModel, Event eventModel)
        {
            eventModel.DayRecurrences.ToList().ForEach(dr => _eventRepository.DeleteEventDayRecurrence(dr.Id));

            eventModel.Name = eventViewModel.Name;
            eventModel.Images = eventViewModel.Images;
            eventModel.Description = eventViewModel.Description;
            eventModel.CategoryId = eventViewModel.CategoryId;
            eventModel.PlaceId = eventViewModel.PlaceId;
            eventModel.RecurringType = eventViewModel.RecurringType;
            eventModel.RecurringUntil = eventViewModel.RecurringUntil;
            eventModel.AdditionalInfo = eventViewModel.AdditionalInfo;
            eventModel.DayRecurrences = eventViewModel.RecurrenceDays.Select(rd =>
                new EventDayRecurrence
                {
                    Day = rd.Day,
                    DayPositions = rd.DayPositions != null ? rd.DayPositions.Select(dp => new EventDayRecurrence2DayPosition { DayPosition = dp }).ToList()
                        : new List<EventDayRecurrence2DayPosition>()
                }).ToList();
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
                Occurrences = occurrences,
                DayRecurrences = eventViewModel.RecurrenceDays.Select(rd => 
                    new EventDayRecurrence 
                    { 
                        Day = rd.Day,
                        DayPositions = rd.DayPositions != null ? rd.DayPositions.Select(dp => new EventDayRecurrence2DayPosition { DayPosition = dp }).ToList()
                            : new List<EventDayRecurrence2DayPosition>()
                    }).ToList()
            };
        }

        private List<EventOccurrence> GenerateOccurrences(EventViewModel viewModel)
        {
            var resultOccurrences = new List<EventOccurrence>();
            var currentPeriodStart = GetCurrentPeriodStart(viewModel.Time, (RecurringType)viewModel.RecurringType);
            var currentTimeOffset = viewModel.Time - currentPeriodStart;
            var endTime = viewModel.RecurringUntil;

            while (currentPeriodStart <= endTime)
            {
                var currentOccurrences = viewModel.RecurrenceDays.Count == 0 ? 
                    new List<EventOccurrence> { new EventOccurrence { Time = currentPeriodStart + currentTimeOffset } }
                    : GetCurrentTimeOccurrences(currentPeriodStart, viewModel);
                resultOccurrences = resultOccurrences.Concat(currentOccurrences).ToList();

                currentPeriodStart += viewModel.RecurringType switch
                {
                    RecurringType.Daily => TimeSpan.FromDays(1),
                    RecurringType.Weekly => TimeSpan.FromDays(7),
                    RecurringType.Biweekly => TimeSpan.FromDays(14),
                    RecurringType.Monthly => (currentPeriodStart.AddMonths(1) - currentPeriodStart),
                    _ => throw new ArgumentException("Invalid recurring type")
                };
            }

            return resultOccurrences.DistinctBy(oc => oc.Time).ToList();
        }

        private DateTime GetCurrentPeriodStart(DateTime currentDate, RecurringType recurringType)
        {
            return recurringType switch
            {
                RecurringType.Weekly => currentDate.AddDays(-(int)currentDate.DayOfWeek + 1),
                RecurringType.Biweekly => currentDate.AddDays(-(int)currentDate.DayOfWeek + 1),
                RecurringType.Monthly => currentDate.AddDays(-currentDate.Day + 1),
                _ => currentDate
            };
        }

        private List<EventOccurrence> GetCurrentTimeOccurrences(DateTime periodStart, EventViewModel viewModel)
        {
            var resultOccurrences = new List<EventOccurrence>();
            int daysInMonth = DateTime.DaysInMonth(periodStart.Year, periodStart.Month);

            foreach (var recurrenceDay in viewModel.RecurrenceDays)
            {
                if (recurrenceDay.DayPositions != null)
                {
                    if (recurrenceDay.DayPositions.Count == 0)
                    {
                        recurrenceDay.DayPositions.Add(DayPosition.First);
                    }

                    var firstDayOfWeekOccurrence = GetFirstDayOfWeekOccurrence(periodStart, (DayOfWeek)recurrenceDay.GetDayOfWeek());
                    foreach (var dayPosition in recurrenceDay.DayPositions)
                    {
                        var occurrenceDayOfMonth = firstDayOfWeekOccurrence.Day + ((int)dayPosition - 1) * amountOfDaysInWeek;
                        if (occurrenceDayOfMonth <= daysInMonth)
                        {
                            resultOccurrences.Add(new EventOccurrence { Time = periodStart.AddDays(occurrenceDayOfMonth - 1) });
                        }
                    }
                }
                else
                {
                    int amountOfDaysOffset = 0;
                    if (viewModel.RecurringType == RecurringType.Weekly || viewModel.RecurringType == RecurringType.Biweekly)
                    {
                        amountOfDaysOffset = (int)recurrenceDay.GetDayOfWeek();
                    }
                    else if (recurrenceDay.Day < (int)DayOfRange.LastDayOfMonth)
                    {
                        amountOfDaysOffset = recurrenceDay.Day;
                    }
                    else if (recurrenceDay.Day == (int)DayOfRange.LastDayOfMonth)
                    {
                        amountOfDaysOffset = daysInMonth;
                    }
                    resultOccurrences.Add(new EventOccurrence { Time = periodStart.AddDays(amountOfDaysOffset - 1) });
                }
            }
            return resultOccurrences.Where(oc => oc.Time >= viewModel.Time && oc.Time <= viewModel.RecurringUntil).ToList();
        }

        private DateTime GetFirstDayOfWeekOccurrence(DateTime firstDayOfMonth, DayOfWeek targetDayOfWeek)
        {
            int daysOffset = targetDayOfWeek - firstDayOfMonth.DayOfWeek;
            return firstDayOfMonth.AddDays(daysOffset < 0 ? daysOffset + amountOfDaysInWeek : daysOffset);
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

        public List<DayOptionViewModel> GetWeeklyRecurrenceOptions()
        {
            List<DayOptionViewModel> dayOfRangeOptions = new List<DayOptionViewModel>();

            foreach (DayOfRange day in Enum.GetValues(typeof(DayOfRange)))
            {
                dayOfRangeOptions.Add(new DayOptionViewModel() { Name = day.ToString(), Value = (int)day });
            }
            return dayOfRangeOptions.Skip(1).ToList();
        }
        
        public List<DayOptionViewModel> GetMonthlyRecurrenceOptions()
        {
            const int firstDayOfMonth = 1;
            List<DayOptionViewModel> monthlyOptions = new List<DayOptionViewModel>();

            //Add days of month
            for(int i = firstDayOfMonth; i < (int)DayOfRange.LastDayOfMonth; i++)
            {
                monthlyOptions.Add(new DayOptionViewModel() { Name = i.ToString(), Value = i });
            }

            //Add days of week with positions + last day of month option
            foreach (DayOfRange day in Enum.GetValues(typeof(DayOfRange)))
            {
                DayOptionViewModel dayOption = new DayOptionViewModel() { Name = $"{day}s", Value = (int)day };
                if (day == DayOfRange.LastDayOfMonth)
                {
                    dayOption.Name = "Last Day";
                }
                else
                {
                    dayOption.DayPositions = new List<DayOptionViewModel>();
                    foreach (DayPosition position in Enum.GetValues(typeof(DayPosition)))
                    {
                        dayOption.DayPositions.Add(new DayOptionViewModel() { Name = position.ToString(), Value = (int)position });
                    }
                }

                monthlyOptions.Add(dayOption);
            }
            return monthlyOptions;
        }
    }
}
