using JobBoard.Application.DTOs.NotificationDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Interfaces
{
    public interface INotificationService
    {
        Task AddNotificationAsync(string userId, string message, string? link = null);
        Task MarkAsReadAsync(int notificationId);
        Task<IEnumerable<NotificationDto>> GetUserNotificationsAsync(string userId);
        Task MarkAllAsReadAsync(string userId);
        Task DeleteNotificationAsync(int notificationId);



    }
}