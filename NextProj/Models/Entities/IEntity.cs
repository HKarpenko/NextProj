namespace Task0.Models.Entities
{
    public interface IEntity<T>
    {
        T Id { get; set; }
    }
}
