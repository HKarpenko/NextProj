namespace Domain.Models.Dtos
{
    public class NotificationDto
    {
        public long SubscriptionId { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public DateTime SendDate { get; set; }
    }
}
