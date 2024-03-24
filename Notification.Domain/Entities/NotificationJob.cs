namespace Notification.Domain.Entities
{
    public class NotificationJob
    {
        public long Id { get; set; }
        public string JobId { get; set; }
        public long? SubscriptionId { get; set; }
    }
}
