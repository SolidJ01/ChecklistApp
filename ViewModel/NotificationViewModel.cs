using ChecklistApp.Model;

namespace ChecklistApp.ViewModel;

public class NotificationViewModel : ViewModel
{
    public enum TimeScale
    {
        Minutes, 
        Hours, 
        Days
    }
    
    private Notification _notification;
    private TimeScale _scale;

    public int Value
    {
        get
        {
            switch (Scale)
            {
                case TimeScale.Minutes:
                    return _notification.Value.Minutes;
                case TimeScale.Hours:
                    return _notification.Value.Hours;
                case TimeScale.Days:
                    return _notification.Value.Days;
            }

            return 0;
        }
        set
        {
            switch (Scale)
            {
                case TimeScale.Minutes:
                    _notification.Value = new TimeSpan(0, value, 0);
                    break;
                case TimeScale.Hours:
                    _notification.Value = new TimeSpan(value,0, 0);
                    break;
                case TimeScale.Days:
                    _notification.Value = new TimeSpan(value, 0, 0, 0);
                    break;
            }
            OnPropertyChanged(nameof(Value));
        }
    }

    public TimeScale Scale
    {
        get { return _scale; }
        set
        {
            int val = Value;
            _scale = value;
            Value = val;
            OnPropertyChanged(nameof(Value));
            OnPropertyChanged(nameof(Scale));
        }
    }

    public NotificationViewModel(Notification notification)
    {
        _notification = notification;
    }
}