namespace Notification.Domain.Dtos
{
    public class EventSubscriptionDto
    {
        public long SubscriptionId { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public DateTime SendDate { get; set; }
    }
}
