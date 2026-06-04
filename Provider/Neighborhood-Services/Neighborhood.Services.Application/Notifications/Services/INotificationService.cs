using Neighborhood.Services.Application.Notifications.Push_inApp.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Neighborhood.Services.Application.Notifications.Services
{
    public interface INotificationService
    {
        Task<PushNotificationDto> SendNotificationAsync(string message);
        Task<PushNotificationDto> SendNotificationToAdmin(string message);
        Task<PushNotificationDto> SendNotificationToUser(string userId, string message);

        Task<PushNotificationDto> SendRoleBasedNotificationAsync(string message, string role, string? recipientUserId = null);
    }
}
