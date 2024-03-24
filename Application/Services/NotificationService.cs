using Application.Interfaces;
using Domain.Models.Dtos;
using System.Text;
using System.Text.Json;

namespace Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IHttpClientFactory _clientFactory;

        public NotificationService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<bool> AddNewNotification(NotificationDto notification)
        {
            try
            {
                var client = _clientFactory.CreateClient("NotificationService");
                var response = await client.PostAsync("api/notification",
                    new StringContent(JsonSerializer.Serialize(notification), Encoding.UTF8, "application/json")
                );

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsByteArrayAsync();
                    var isNotificationAdded = JsonSerializer.Deserialize<bool>(result);

                    return isNotificationAdded;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdateNotification(NotificationDto notification)
        {
            try
            {
                var client = _clientFactory.CreateClient("NotificationService");
                var response = await client.PutAsync("api/notification",
                    new StringContent(JsonSerializer.Serialize(notification), Encoding.UTF8, "application/json")
                );

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsByteArrayAsync();
                    var isNotificationUpdated = JsonSerializer.Deserialize<bool>(result);

                    return isNotificationUpdated;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DeleteNotification(long subscriptionId)
        {
            try
            {
                var client = _clientFactory.CreateClient("NotificationService");
                var response = await client.DeleteAsync($"api/notification/{subscriptionId}");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsByteArrayAsync();
                    var isNotificationDeleted = JsonSerializer.Deserialize<bool>(result);

                    return isNotificationDeleted;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
