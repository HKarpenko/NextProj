using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Notification.Application.Interfaces;
using Notification.Domain.Dtos;
using Notification.Domain.Entities;
using Notification.Infrastructure.Repositories.Interfaces;
using Serilog;

namespace Notification.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(IServiceProvider serviceProvider, INotificationRepository notificationRepository)
        {
            _serviceProvider = serviceProvider;
            _notificationRepository = notificationRepository;
        }

        public bool ScheduleEmailJob(EventSubscriptionDto eventSubscription)
        {
            try
            {
                var emailService = _serviceProvider.GetRequiredService<IEmailService>();
                string jobId = BackgroundJob.Schedule(() =>
                    emailService.SendEmailAsync(eventSubscription.Email, eventSubscription.Message), eventSubscription.SendDate);
                var newNotificationJob = new NotificationJob
                {
                    JobId = jobId,
                    SubscriptionId = eventSubscription.SubscriptionId
                };

                _notificationRepository.AddNotificationJob(newNotificationJob);
                _notificationRepository.SaveChanges();
                Log.Debug($"New notification job scheduled, jobId: {jobId}, subscriptionId: {eventSubscription.SubscriptionId}");
                
                return true;
            }
            catch (Exception ex)
            {
                Log.Error($"Problem occured during creating notification, subscriptionId: {eventSubscription.SubscriptionId}, error: {ex}");
                return false;
            }
        }

        public bool UpdateScheduledEmailJob(EventSubscriptionDto eventSubscription)
        {
            try
            {
                var notificationJob = eventSubscription.SubscriptionId == null ? null
                    : _notificationRepository.GetNotificationJobBySubscriptionId(eventSubscription.SubscriptionId);
                if (notificationJob != null)
                {
                    BackgroundJob.Delete(notificationJob.JobId);
                    notificationJob.JobId = BackgroundJob.Schedule<EmailService>(x =>
                        x.SendEmailAsync(eventSubscription.Email, eventSubscription.Message), eventSubscription.SendDate);

                    _notificationRepository.SaveChanges();
                    Log.Debug($"Notification job updated, jobId: {notificationJob.JobId}, subscriptionId: {notificationJob.SubscriptionId}");
                    return true;
                }

                Log.Debug($"Notification job not found, subscriptionId: {eventSubscription.SubscriptionId}");
                return false;
            }
            catch(Exception ex)
            {
                Log.Error($"Problem occured during updating notification, subscriptionId: {eventSubscription.SubscriptionId}, error: {ex}");
                return false;
            }

        }

        public bool RemoveScheduledEmailJob(long eventSubscriptionId)
        {
            try
            {
                var notificationJob = _notificationRepository.GetNotificationJobBySubscriptionId(eventSubscriptionId);
                if (notificationJob != null)
                {
                    BackgroundJob.Delete(notificationJob.JobId);
                    _notificationRepository.RemoveNotificationJob(notificationJob.Id);

                    _notificationRepository.SaveChanges();
                    Log.Debug($"Notification job deleted, jobId: {notificationJob.JobId}, subscriptionId: {notificationJob.SubscriptionId}");

                    return true;
                }

                Log.Debug($"Notification job not found, subscriptionId: {eventSubscriptionId}");
                return false;
            }
            catch(Exception ex )
            {
                Log.Error($"Problem occured during deleting notification, subscriptionId: {eventSubscriptionId}, error: {ex}");
                return false;
            }
        }

        public bool ScheduleEmailJobs(List<EventSubscriptionDto> eventSubscription)
        {
            throw new NotImplementedException();
        }

        public bool UpdateScheduledEmailJobs(List<EventSubscriptionDto> eventSubscription)
        {
            throw new NotImplementedException();
        }

        public bool RemoveScheduledEmailJobs(List<long> eventSubscriptionId)
        {
            throw new NotImplementedException();
        }
    }
}
