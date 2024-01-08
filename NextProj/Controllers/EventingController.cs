using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NextProj.Models.Entities;
using NextProj.Repositories;

namespace Task0.Controllers
{
    public class EventingController : Controller
    {
        private readonly IEventRepository _eventRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IPlaceRepository _placeRepository;

        public EventingController(IEventRepository eventRepository, ICategoryRepository categoryRepository,
            IPlaceRepository placeRepository) 
        {
            _eventRepository = eventRepository;
            _categoryRepository = categoryRepository;
            _placeRepository = placeRepository;
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
            var places = _placeRepository.GetAll();
            var categories = _categoryRepository.GetAll();

            ViewData["Places"] = new SelectList(places, "Id", "DisplayName");
            ViewData["Categories"] = new SelectList(categories, "Id", "Name");

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
                currentEvent.CategoryId = eventModel.CategoryId;
                currentEvent.Description = eventModel.Description;
                currentEvent.PlaceId = eventModel.PlaceId;
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
