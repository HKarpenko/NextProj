using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NextProj.Models.ViewModels;
using NextProj.Repositories;
using NextProj.Services;

namespace Task0.Controllers
{
    public class EventingController : Controller
    {
        private readonly IEventService _eventService;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IPlaceRepository _placeRepository;

        public EventingController(IEventService eventService, ICategoryRepository categoryRepository,
            IPlaceRepository placeRepository) 
        {
            _eventService = eventService;
            _categoryRepository = categoryRepository;
            _placeRepository = placeRepository;
        }

        public IActionResult EventsList()
        {
            ViewData["Places"] = _placeRepository.GetAll();
            ViewData["Categories"] = _categoryRepository.GetAll();

            var events = _eventService.GetAllEvents();

            return View(events);
        }

        [HttpGet]
        public IActionResult Details(long id)
        {
            var eventModel = _eventService.GetEventById(id);

            return View(eventModel);
        }


        [HttpGet]
        public IActionResult Create()
        {
            var places = _placeRepository.GetAll();
            var categories = _categoryRepository.GetAll();

            ViewData["Places"] = new SelectList(places, "Id", "DisplayName");
            ViewData["Categories"] = new SelectList(categories, "Id", "Name");

            return View();
        }

        [HttpPost]
        public IActionResult Create(EventViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                _eventService.AddEvent(viewModel);
                return RedirectToAction("EventsList");
            }

            return View();
        }

        [HttpGet]
        public IActionResult Edit(long id)
        {
            var eventModel = _eventService.GetEventById(id);
            var places = _placeRepository.GetAll();
            var categories = _categoryRepository.GetAll();

            ViewData["Places"] = new SelectList(places, "Id", "DisplayName");
            ViewData["Categories"] = new SelectList(categories, "Id", "Name");

            return View(eventModel);
        }

        [HttpPost]
        public IActionResult Edit([FromBody] SaveEventViewModel model)
        {
            if (ModelState.IsValid)
            {
                _eventService.EditEvent(model);

                return Json(new { redirectTo = Url.Action("EventsList") });
            }

            return View(model);
        }

        public IActionResult Delete(long occurenceId, bool isSeries)
        {
            _eventService.DeleteEventOccurrences(occurenceId, isSeries);

            return RedirectToAction("EventsList");
        }

        public IActionResult FullCalendar()
        {
            var events = _eventService.GetAllEvents();

            return View(events);
        }
    }
}