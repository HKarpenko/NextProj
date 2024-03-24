using Notification.Domain.Entities;
using Notification.Infrastructure.Data;
using Notification.Infrastructure.Repositories.Interfaces;

namespace Notification.Infrastructure.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly AppDbContext _context;

        public NotificationRepository(AppDbContext context)
        {
            _context = context;
        }

        public NotificationJob GetNotificationJobById(long id)
        {
            return _context.NotificationJobs.FirstOrDefault(nj => nj.Id == id);
        }

        public NotificationJob GetNotificationJobBySubscriptionId(long subscriptionId)
        {
            return _context.NotificationJobs.FirstOrDefault(nj => nj.SubscriptionId == subscriptionId);
        }

        public void AddNotificationJob(NotificationJob notificationJob)
        {
            _context.NotificationJobs.Add(notificationJob);
        }

        public void RemoveNotificationJob(long id)
        {
            var notificationJob = GetNotificationJobById(id);
            _context.NotificationJobs.Remove(notificationJob);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
