using NextProj.Models.Enums;

namespace NextProj.Models.Entities
{
    public class Event : IEntity<long>
    {
        public long Id { get; set; }
        public long? CategoryId { get; set; }
        public virtual Category Category {  get; set; }
        public string Name { get; set; }
        public string Images { get; set; }
        public string Description { get; set; }
        public long? PlaceId { get; set; }
        public virtual Place Place { get; set; }
        public RecurringType? RecurringType { get; set; }
        public DateTime? RecurringUntil { get; set; }
        public string AdditionalInfo { get; set; }
        public virtual IEnumerable<EventOccurrence> Occurrences { get; set; }
    }
}
