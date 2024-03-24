using Microsoft.AspNetCore.Mvc;
using Notification.Application.Interfaces;
using Notification.Domain.Dtos;
using Serilog;

namespace Notification.WebApi.Controllers
{
    [ApiController]
    [Route("api/notification")]
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost]
        public IActionResult Create([FromBody] EventSubscriptionDto eventSubscription)
        {
            Log.Debug($"Create notification job");
            var result = _notificationService.ScheduleEmailJob(eventSubscription);
            return Ok(result);
        }

        [HttpPut]
        public IActionResult Update([FromBody] EventSubscriptionDto eventSubscription)
        {
            Log.Debug($"Update notification job");
            var result = _notificationService.UpdateScheduledEmailJob(eventSubscription);
            return Ok(result);
        }

        [HttpDelete("{subscriptionId}")]
        public IActionResult Delete(long subscriptionId)
        {
            Log.Debug($"Delete notification job");
            var result = _notificationService.RemoveScheduledEmailJob(subscriptionId);
            return Ok(result);
        }
    }
}
