using Domain.Models.Entities;
using Domain.Models.ViewModels;
using Application.Interfaces;
using Infrastructure.Repositories.Interfaces;
using Domain.Models.Dtos;

namespace Application.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IEventRepository _eventRepository;
        private readonly INotificationService _notificationService;

        public SubscriptionService(ISubscriptionRepository subscriptionRepository, IEventRepository eventRepository, INotificationService notificationService)
        {
            _subscriptionRepository = subscriptionRepository;
            _eventRepository = eventRepository;
            _notificationService = notificationService;
        }

        public List<EventSubscriptionViewModel> GetSubscriptionsByOccurrenceId(long occurrenceId)
        {
            var eventSubscriptions = _subscriptionRepository.GetSubscriptionsByOccurrenceId(occurrenceId);
            return eventSubscriptions.Select(es => new EventSubscriptionViewModel
            {
                Id = es.Id,
                Name = es.Name,
                ReceiverEmail = es.ReceiverEmail,
                NotificationTime = es.NotificationTime.ToString(),
                Message = es.Message
            }).ToList();
        }

        public void AddSubscription(SaveEventSubscriptionViewModel saveModel)
        {
            var eventModel = _eventRepository.GetEventByOccurrenceId(saveModel.OccurrenceId);
            var eventOccurrence = eventModel.Occurrences.First(oc => oc.Id == saveModel.OccurrenceId);

            if (saveModel.IsSeries)
            {
                eventModel.Occurrences.ToList().ForEach(oc =>
                {
                    var newSubscription = CreateEventSubscription(saveModel, eventOccurrence.Time);
                    newSubscription.EventOccurrenceId = oc.Id;
                    tryAddSubscriptionIfNotExists(oc, newSubscription);
                });
            }
            else
            {
                var newSubscription = CreateEventSubscription(saveModel, eventOccurrence.Time);
                tryAddSubscriptionIfNotExists(eventOccurrence, newSubscription);
            }
        }

        private EventSubscription CreateEventSubscription(SaveEventSubscriptionViewModel saveModel, DateTime occurrenceTime)
        {
            var subscription = new EventSubscription();

            SetFieldsFromViewToSubscription(saveModel, subscription, occurrenceTime);
            subscription.EventOccurrenceId = saveModel.OccurrenceId;

            return subscription;
        }

        private void SetFieldsFromViewToSubscription(SaveEventSubscriptionViewModel saveModel,
            EventSubscription subscription, DateTime occurrenceTime)
        {
            subscription.Name = saveModel.Name;
            subscription.ReceiverEmail = saveModel.ReceiverEmail;
            subscription.NotificationTime = saveModel.NotificationTime == null ? TimeOnly.FromTimeSpan(occurrenceTime.TimeOfDay)
                : TimeOnly.Parse(saveModel.NotificationTime);
            subscription.Message = saveModel.Message;
        }

        private async void tryAddSubscriptionIfNotExists(EventOccurrence eventOccurrence, EventSubscription newSubscription)
        {
            var subscriptions = eventOccurrence.Subscriptions.ToList();
            if (!subscriptions.Any(ocs => CheckSubscriptionsEqual(ocs, newSubscription)))
            {
                await _subscriptionRepository.AddAndSaveNewSubscription(newSubscription);
                var result = await _notificationService.AddNewNotification(CreateNotificationDto(eventOccurrence, newSubscription));

                if (!result)
                {
                    await _subscriptionRepository.DeleteSubscription(newSubscription.Id);
                }
            }
        }

        private bool CheckSubscriptionsEqual(EventSubscription subscription1, EventSubscription subscription2)
        {
            return subscription1.Name == subscription2.Name &&
                subscription1.ReceiverEmail == subscription2.ReceiverEmail &&
                subscription1.NotificationTime == subscription2.NotificationTime &&
                subscription1.Message == subscription2.Message;
        }

        private NotificationDto CreateNotificationDto(EventOccurrence eventOccurrence, EventSubscription newSubscription)
        {
            return new NotificationDto
            {
                SubscriptionId = newSubscription.Id,
                SendDate = new DateTime(eventOccurrence.Time.Year, eventOccurrence.Time.Month, eventOccurrence.Time.Day,
                        newSubscription.NotificationTime.Hour, newSubscription.NotificationTime.Minute, newSubscription.NotificationTime.Second),
                Email = newSubscription.ReceiverEmail,
                Message = newSubscription.Message
            };
        }

        public async void UpdateSubscription(SaveEventSubscriptionViewModel saveModel)
        {
            var subscription = _subscriptionRepository.GetSubscriptionById(saveModel.Id);

            if (saveModel.IsSeries)
            {
                GetAllRelatedEventSubscriptions(subscription.EventOccurrence.EventId, subscription).ForEach(async s =>
                {
                    var originalSubsciption = s.GetCopy();

                    SetFieldsFromViewToSubscription(saveModel, s, subscription.EventOccurrence.Time);
                    _subscriptionRepository.SaveChanges();

                    tryUpdateNotification(s, originalSubsciption);
                });
            }
            else
            {
                var originalSubsciption = subscription.GetCopy();

                SetFieldsFromViewToSubscription(saveModel, subscription, subscription.EventOccurrence.Time);
                _subscriptionRepository.SaveChanges();
                tryUpdateNotification(subscription, originalSubsciption);
            }
        }

        private async void tryUpdateNotification(EventSubscription updatedSubscription, EventSubscription originalSubsciption)
        {
            var occurrence = _eventRepository.GetEventOccurrenceById(updatedSubscription.EventOccurrenceId);
            var notificationDto = CreateNotificationDto(occurrence, updatedSubscription);
            var result = await _notificationService.UpdateNotification(notificationDto);

            if (!result)
            {
                await _subscriptionRepository.UpdateAndSaveSubscription(originalSubsciption);
            }
        }

        public async Task<long> DeleteSubscription(long subscriptionId, bool isSeries)
        {
            var subscription = _subscriptionRepository.GetSubscriptionById(subscriptionId);
            var eventOccurrenceId = subscription.EventOccurrenceId;
            if (isSeries)
            {
                var relatedSubscriptions = GetAllRelatedEventSubscriptions(subscription.EventOccurrence.EventId, subscription);
                foreach (var relatedSubscription in relatedSubscriptions)
                {
                    await _subscriptionRepository.DeleteSubscription(relatedSubscription.Id);
                    var result = await _notificationService.DeleteNotification(relatedSubscription.Id);
                    if(!result)
                    {
                        _subscriptionRepository.SaveChanges();
                    }
                }
            }
            else
            {
                await _subscriptionRepository.DeleteSubscription(subscriptionId);
                var result = await _notificationService.DeleteNotification(subscriptionId);
                if (!result)
                {
                    _subscriptionRepository.SaveChanges();
                }
            }
            return eventOccurrenceId;
        }

        private List<EventSubscription> GetAllRelatedEventSubscriptions(long eventId, EventSubscription mainSubscription)
        {
            var eventModel = _eventRepository.GetEventById(eventId);
            var resultSubscriptions = eventModel.Occurrences.ToList().SelectMany(occurrence =>
            {
                return occurrence.Subscriptions.Where(s => CheckSubscriptionsEqual(s, mainSubscription));
            }).ToList();

            return resultSubscriptions;
        }
    }
}
