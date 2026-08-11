namespace ChecklistApp.Model;

public class ColorSelectRequestEventArgs(int? id, Color? color, Action<int?, Color> callback) : EventArgs
{
    public int? Id => id;
    public Color? Color => color;
    public Action<int?, Color> Callback => callback;
}