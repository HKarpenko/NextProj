using Notification.Domain.Dtos;

namespace Notification.Application.Interfaces
{
    public interface INotificationService
    {
        bool ScheduleEmailJob(EventSubscriptionDto eventSubscription);
        bool UpdateScheduledEmailJob(EventSubscriptionDto eventSubscription);
        bool RemoveScheduledEmailJob(long eventSubscriptionId);
        bool ScheduleEmailJobs(List<EventSubscriptionDto> eventSubscription);
        bool UpdateScheduledEmailJobs(List<EventSubscriptionDto> eventSubscription);
        bool RemoveScheduledEmailJobs(List<long> eventSubscriptionId);

    }
}