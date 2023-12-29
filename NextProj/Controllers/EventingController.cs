using Microsoft.AspNetCore.Mvc;
using NextProj.Models.Entities;
using NextProj.Repositories;

namespace Task0.Controllers
{
    public class EventingController : Controller
    {
        private readonly IEventRepository _eventRepository;

        public EventingController(IEventRepository eventRepository) 
        {
            _eventRepository = eventRepository;
        }

        public IActionResult EventsList()
        {
            var events = _eventRepository.GetAllEvents();

            return View(events);
        }

        [HttpGet]
        public IActionResult Details(long id)
        {
            var eventModel = _eventRepository.GetEventById(id);

            return View(eventModel);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Event eventModel)
        {
            if (ModelState.IsValid)
            {
                var newEvent = new Event
                {
                    Name = eventModel.Name,
                    AdditionalInfo = eventModel.AdditionalInfo,
                    Category = eventModel.Category,
                    Description = eventModel.Description,
                    Place = eventModel.Place,
                    Time = eventModel.Time,
                    Images = eventModel.Images
                };
                _eventRepository.AddEvent(newEvent);

                return RedirectToAction("EventsList");
            }

            return View();
        }

        [HttpGet]
        public IActionResult Edit(long id)
        {
            var eventModel = _eventRepository.GetEventById(id);

            return View(eventModel);
        }

        [HttpPost]
        public IActionResult Edit(Event eventModel)
        {
            if (ModelState.IsValid)
            {
                var currentEvent = _eventRepository.GetEventById(eventModel.Id);

                currentEvent.Name = eventModel.Name;
                currentEvent.AdditionalInfo = eventModel.AdditionalInfo;
                currentEvent.Category = eventModel.Category;
                currentEvent.Description = eventModel.Description;
                currentEvent.Place = eventModel.Place;
                currentEvent.Time = eventModel.Time;
                currentEvent.Images = eventModel.Images;

                _eventRepository.SaveEvent(currentEvent);

                return RedirectToAction(nameof(EventsList));
            }

            return View(eventModel);
        }

        public IActionResult Delete(long id)
        {
            _eventRepository.DeleteEvent(id);

            return RedirectToAction(nameof(EventsList));
        }
    }
}
