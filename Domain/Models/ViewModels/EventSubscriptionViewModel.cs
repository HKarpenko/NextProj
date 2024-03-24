using System.ComponentModel.DataAnnotations;

namespace Domain.Models.ViewModels
{
    public class EventSubscriptionViewModel
    {
        public long Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        public string? Message { get; set; }
        [EmailAddress]
        public string ReceiverEmail { get; set; }
        public string? NotificationTime { get; set; }
    }

    public class SaveEventSubscriptionViewModel : EventSubscriptionViewModel
    {
        public bool IsSeries { get; set; }
        public long OccurrenceId { get; set; }
    }
}
