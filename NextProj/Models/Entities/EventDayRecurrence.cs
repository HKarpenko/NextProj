namespace NextProj.Models.Entities
{
    public class EventDayRecurrence : IEntity<long>
    {
        public long Id { get; set; }
        public long EventId { get; set; }
        public virtual Event Event { get; set; }
        public int Day { get; set; }
        public virtual List<EventDayRecurrence2DayPosition> DayPositions { get; set; }
    }
}