namespace ChecklistApp.Services;

public interface INotificationManagerService
{
    event EventHandler NotificationReceived;
    void SendNotification(string title, string message, DateTime? notifyTime = null);
    void CancelNotification();
    void ReceiveNotification(string title, string message);
}