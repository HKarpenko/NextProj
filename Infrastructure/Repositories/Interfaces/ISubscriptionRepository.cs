using Domain.Models.Entities;

namespace Infrastructure.Repositories.Interfaces
{
    public interface ISubscriptionRepository : IBaseRepository
    {
        IEnumerable<EventSubscription> GetSubscriptionsByOccurrenceId(long id);
        EventSubscription GetSubscriptionById(long subscriptionId);
        Task<long> AddAndSaveNewSubscription(EventSubscription subscription);
        Task<long> UpdateAndSaveSubscription(EventSubscription subscription);
        Task DeleteSubscription(long subscriptionId);
        Task SaveChangesAsync();
    }
}