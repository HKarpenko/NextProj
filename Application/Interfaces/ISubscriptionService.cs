using Domain.Models.ViewModels;

namespace Application.Interfaces
{
    public interface ISubscriptionService
    {
        Task<long> DeleteSubscription(long subscriptionId, bool isSeries);
        List<EventSubscriptionViewModel> GetSubscriptionsByOccurrenceId(long occurrenceId);
        void AddSubscription(SaveEventSubscriptionViewModel saveModel);
        void UpdateSubscription(SaveEventSubscriptionViewModel saveModel);
    }
}