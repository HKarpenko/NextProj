namespace Domain.Models.Entities
{
    public interface IEntity<T>
    {
        T Id { get; set; }
    }
}
