using Notification.Domain.Entities;

namespace Notification.Infrastructure.Repositories.Interfaces
{
    public interface INotificationRepository
    {
        void AddNotificationJob(NotificationJob notificationJob);
        NotificationJob GetNotificationJobBySubscriptionId(long id);
        NotificationJob GetNotificationJobById(long id);
        void RemoveNotificationJob(long id);
        void SaveChanges();
    }
}