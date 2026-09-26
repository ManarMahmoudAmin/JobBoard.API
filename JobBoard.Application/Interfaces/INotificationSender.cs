namespace JobBoard.Application.Interfaces
{
    public interface INotificationSender
    {
        Task SendNotificationAsync(string userId, string message, string? link = null);
        Task SendNotificationUpdateAsync(string userId, object updateData);

    }
}
