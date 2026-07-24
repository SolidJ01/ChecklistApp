namespace ChecklistApp.Model;

public class ToastAnchorChangeEventArgs(ToastAnchorChangeEventArgs.Intent intent, Rect value) : EventArgs
{
    public enum Intent
    {
        Apply, 
        Remove
    }

    public Intent EventIntent =>  intent;
    public Rect Value => value;
}