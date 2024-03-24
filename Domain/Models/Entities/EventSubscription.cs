using System.ComponentModel.DataAnnotations;

namespace Domain.Models.Entities
{
    public class EventSubscription : IEntity<long>
    {
        public long Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        [EmailAddress]
        public string ReceiverEmail { get; set; }
        public string? Message { get; set; }
        public TimeOnly NotificationTime { get; set; }
        public long EventOccurrenceId { get; set; }
        public virtual EventOccurrence EventOccurrence { get; set; }

        public EventSubscription GetCopy()
        {
            return new EventSubscription()
            {
                Id = Id,
                EventOccurrenceId = EventOccurrenceId,
                Message = Message,
                Name = Name,
                ReceiverEmail = ReceiverEmail,
                NotificationTime = NotificationTime
            };
        }
    }
}