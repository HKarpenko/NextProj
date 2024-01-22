using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NextProj.Models.ViewModels;
using NextProj.Services;

namespace Task0.Controllers
{
    public class EventingController : Controller
    {
        private readonly IEventService _eventService;
        private readonly ICategoryService _categoryService;
        private readonly IPlaceService _placeService;

        public EventingController(IEventService eventService, ICategoryService categoryService,
            IPlaceService placeService) 
        {
            _eventService = eventService;
            _categoryService = categoryService;
            _placeService = placeService;
        }

        public IActionResult EventsList()
        {
            ViewData["Places"] = _placeService.GetAllPlaces();
            ViewData["Categories"] = _categoryService.GetAllCategories();

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
            FillSelectionLists();
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
            FillSelectionLists();

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

        private void FillSelectionLists()
        {
            var places = _placeService.GetAllPlaces();
            var categories = _categoryService.GetAllCategories();

            ViewData["Places"] = new SelectList(places, "Id", "DisplayName");
            ViewData["Categories"] = new SelectList(categories, "Id", "Name");
        }
    }
}