namespace Domain.Models.Entities
{
    public class Place : IEntity<long>
    {
        public long Id { get; set; }
        public string DisplayName { get => $"{Address}, {PostalCode}, {City}, {Country}"; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
    }
}
