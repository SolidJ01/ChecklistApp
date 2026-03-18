namespace ChecklistApp.Services;

public class ToastService
{
    public event EventHandler ToastProposed;
    
    private Queue<string> _toastQueue = [];
    
    public bool _toastsQueued { get { return _toastQueue.Count > 0; } }

    public void QueueToast(string toast)
    {
        _toastQueue.Enqueue(toast);
        ToastProposed?.Invoke(this, EventArgs.Empty);
    }

    public string GetToast()
    {
        try
        {
            return _toastQueue.Dequeue();
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }
}