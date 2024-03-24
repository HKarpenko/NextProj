using Domain.Models.Dtos;

namespace Application.Interfaces
{
    public interface INotificationService
    {
        Task<bool> AddNewNotification(NotificationDto notification);
        Task<bool> DeleteNotification(long subscriptionId);
        Task<bool> UpdateNotification(NotificationDto notification);
    }
}