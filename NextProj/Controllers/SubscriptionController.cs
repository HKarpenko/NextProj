using Domain.Models.Entities;
using Domain.Models.ViewModels;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Controllers
{
    public class SubscriptionController : Controller
    {
        ISubscriptionService _subscriptionService;

        public SubscriptionController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] SaveEventSubscriptionViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Id == 0)
                {
                    await _subscriptionService.AddSubscription(model);
                }
                else
                {
                    await _subscriptionService.UpdateSubscription(model);
                }

                return Json(new { redirectTo = Url.Action("Details", "Eventing", new { id = model.OccurrenceId }) });
            }

            return Problem("Model validity check failed");
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromQuery] long subscriptionId, [FromQuery] bool isSeries)
        {
            long occurrenceId = await _subscriptionService.DeleteSubscription(subscriptionId, isSeries);

            return Json(new { redirectTo = Url.Action("Details", "Eventing", new { id = occurrenceId }) });
        }
    }
}
