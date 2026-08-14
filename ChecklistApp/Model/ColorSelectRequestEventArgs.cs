namespace ChecklistApp.Model;

public class ColorSelectRequestEventArgs(Color? color, Action<Color> callback, Action? deleteCallback = null) : EventArgs
{
    public Color? Color => color;
    public Action<Color> Callback => callback;
    public Action? DeleteCallback => deleteCallback;
}