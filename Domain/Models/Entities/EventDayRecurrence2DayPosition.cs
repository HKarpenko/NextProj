using Domain.Models.Enums;

namespace Domain.Models.Entities
{
    public class EventDayRecurrence2DayPosition : IEntity<long>
    {
        public long Id { get; set; }
        public DayPosition DayPosition { get; set; }
        public long EventDayRecurrenceId { get; set; }
        public virtual EventDayRecurrence EventDayRecurrence { get; set; }
    }
}
