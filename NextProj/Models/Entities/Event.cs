namespace Task0.Models.Entities
{
    public class Event : IEntity<long>
    {
        public long Id { get; set; }
        public string Category {  get; set; }
        public string Name { get; set; }
        public string Images { get; set; }
        public string Description { get; set; }
        public string Place { get; set; }
        public DateTime Time { get; set; }
        public string AdditionalInfo { get; set; }

    }
}
