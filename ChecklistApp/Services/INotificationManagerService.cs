namespace ChecklistApp.Services;

public interface INotificationManagerService
{
    event EventHandler NotificationReceived;
    //int SendNotification(string title, string message, DateTime? notifyTime = null);
    void SendNotification(int id, string title, string message, DateTime? notifyTime = null);
    void CancelNotification(int id);
    void ReceiveNotification(string title, string message);
}