using Domain.Models.Entities;
using Infrastructure.Repositories.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class SubscriptionRepository : BaseRepository, ISubscriptionRepository
    {
        private readonly AppDbContext _context;

        public SubscriptionRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<EventSubscription> GetSubscriptionsByOccurrenceId(long occurrenceId)
        {
            return _context.EventSubscriptions.Where(es => es.EventOccurrenceId == occurrenceId);
        }

        public EventSubscription GetSubscriptionById(long subscriptionId)
        {
            var subscription = _context.EventSubscriptions
                .Include(es => es.EventOccurrence)
                    .ThenInclude(eo => eo.Subscriptions)
                .FirstOrDefault(es => es.Id == subscriptionId);

            return subscription;
        }

        public async Task<long> AddAndSaveNewSubscription(EventSubscription subscription)
        {
            await _context.EventSubscriptions.AddAsync(subscription);
            await _context.SaveChangesAsync();
            return subscription.Id;
        }

        public async Task<long> UpdateAndSaveSubscription(EventSubscription subscription)
        {
            _context.EventSubscriptions.Update(subscription);
            await _context.SaveChangesAsync();
            return subscription.Id;
        }

        public async Task DeleteSubscription(long subscriptionId)
        {
            var subscription = await _context.EventSubscriptions.FirstOrDefaultAsync(es => es.Id == subscriptionId);
            if (subscription != null)
            {
                _context.EventSubscriptions.Remove(subscription);
                await _context.SaveChangesAsync();
            }
        }
    }
}
