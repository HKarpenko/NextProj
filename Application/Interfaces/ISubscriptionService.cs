using Domain.Models.ViewModels;

namespace Application.Interfaces
{
    public interface ISubscriptionService
    {
        Task<long> DeleteSubscription(long subscriptionId, bool isSeries);
        List<EventSubscriptionViewModel> GetSubscriptionsByOccurrenceId(long occurrenceId);
        Task AddSubscription(SaveEventSubscriptionViewModel saveModel);
        Task UpdateSubscription(SaveEventSubscriptionViewModel saveModel);
    }
}