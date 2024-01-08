namespace NextProj.Models.Entities
{
    public class Category : IEntity<long>
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
