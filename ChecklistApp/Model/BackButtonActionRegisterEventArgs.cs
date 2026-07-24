namespace ChecklistApp.Model;

public class BackButtonActionRegisterEventArgs : EventArgs
{
    public enum Intent
    {
        Register, 
        Deregister
    }

    private Intent _eventIntent;
    private Action _action;
    
    public Intent EventIntent { get { return _eventIntent; } }
    public Action Action { get { return _action; } }

    public BackButtonActionRegisterEventArgs(Intent eventIntent, Action action)
    {
        _eventIntent = eventIntent;
        _action = action;
    }
}