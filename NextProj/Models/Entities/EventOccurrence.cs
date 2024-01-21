namespace NextProj.Models.Entities
{
    public class EventOccurrence : IEntity<long>
    {
        public long Id { get; set; }
        public DateTime Time { get; set; }
        public long EventId { get; set; }
        public virtual Event Event { get; set; }
    }
}
