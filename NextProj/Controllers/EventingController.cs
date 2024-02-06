using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Domain.Models.ViewModels;
using Application.Services;

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
            ViewData["TotalPages"] = _eventService.GetEventPagesCount(Request.Query);
            var events = _eventService.GetEventsPage(Request.Query);

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
        public IActionResult Create([FromBody] EventViewModel viewModel)
        {
            if (ModelState.IsValid && CheckEventModelValidity(viewModel))
            {
                _eventService.AddEvent(viewModel);

                return Json(new { redirectTo = Url.Action("EventsList") });
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
            if (ModelState.IsValid && CheckEventModelValidity(model))
            {
                _eventService.EditEvent(model);

                return Json(new { redirectTo = Url.Action("EventsList") });
            }

            return View(model);
        }

        public IActionResult Delete(long occurrenceId, bool isSeries)
        {
            _eventService.DeleteEventOccurrences(occurrenceId, isSeries);

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

            ViewData["weeklyOptions"] = _eventService.GetWeeklyRecurrenceOptions();
            ViewData["monthlyOptions"] = _eventService.GetMonthlyRecurrenceOptions();
            ViewData["Places"] = new SelectList(places, "Id", "DisplayName");
            ViewData["Categories"] = new SelectList(categories, "Id", "Name");
        }

        private bool CheckEventModelValidity(EventViewModel viewModel)
        {
            return (viewModel.RecurringType == null || viewModel.RecurringUntil != null) &&
                (viewModel.RecurringUntil == null || viewModel.Time < viewModel.RecurringUntil);
        }
    }
}