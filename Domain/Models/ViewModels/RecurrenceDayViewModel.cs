using Domain.Models.Enums;

namespace Domain.Models.ViewModels
{
    public class RecurrenceDayViewModel
    {
        public int Day { get; set; }
        public List<DayPosition>? DayPositions { get; set; }

        public DayOfWeek? GetDayOfWeek()
        {
            return Day >= (int)DayOfRange.Monday ? (DayOfWeek)(Day - (int)DayOfRange.Monday + 1) : null;
        }
    }
}
