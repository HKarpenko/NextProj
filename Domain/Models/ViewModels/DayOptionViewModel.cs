namespace Domain.Models.ViewModels
{
    public class DayOptionViewModel
    {
        public string Name { get; set; }
        public int Value { get; set; }
        public List<DayOptionViewModel> DayPositions { get; set; }
    }
}
