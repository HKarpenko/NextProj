﻿using NextProj.Models.Enums;

namespace NextProj.Models.ViewModels
{
    public class EventViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Images { get; set; }
        public string Description { get; set; }
        public long? CategoryId { get; set; }
        public string? Category { get; set; }
        public long? PlaceId { get; set; }
        public string? Place { get; set; }
        public string AdditionalInfo { get; set; }
        public DateTime Time { get; set; }
        public RecurringType? RecurringType { get; set; }
    }

    public class SaveEventViewModel : EventViewModel
    {
        public bool isSeries {  get; set; }
    }
}