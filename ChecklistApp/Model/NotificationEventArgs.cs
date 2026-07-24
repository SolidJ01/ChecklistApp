namespace ChecklistApp.Model;

public class NotificationEventArgs : EventArgs
{
    private string _title;
    private string _message;
    
    public string Title =>  _title;
    public string Message => _message;

    public NotificationEventArgs(string title, string message)
    {
        _title = title;
        _message = message;
    }
}