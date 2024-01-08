namespace NextProj.Models.Entities
{
    public class Event : IEntity<long>
    {
        public long Id { get; set; }
        public long? CategoryId { get; set; }
        public Category? Category {  get; set; }
        public string Name { get; set; }
        public string Images { get; set; }
        public string Description { get; set; }
        public long? PlaceId { get; set; }
        public Place? Place { get; set; }
        public DateTime Time { get; set; }
        public string AdditionalInfo { get; set; }

    }
}
