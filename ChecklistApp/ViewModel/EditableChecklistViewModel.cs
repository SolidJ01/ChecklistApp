using System.Collections.ObjectModel;
using ChecklistApp.Model;
using CommunityToolkit.Maui.Core.Extensions;

namespace ChecklistApp.ViewModel;

public class EditableChecklistViewModel : ViewModel
{
    private Checklist _checklist;

    public string Name
    {
        get
        {
            return _checklist.Name;
        }
        set
        {
            _checklist.Name = value;
            OnPropertyChanged();
        }
    }

    public Checklist.ChecklistColor Color
    {
        get
        {
            return _checklist.Color;
        }
        set
        {
            _checklist.Color = value;
            OnPropertyChanged();
        }
    }

    public bool UseDeadline
    {
        get 
        {
            return _checklist.UseDeadline;
        }
        set
        {
            _checklist.UseDeadline = value;
            OnPropertyChanged();
        }
    }
    
    public DateTime Deadline 
    {
        get
        {
            return _checklist.Deadline;
        }
        set 
        {
            _checklist.Deadline = value;
            OnPropertyChanged();
        }
    }

    public bool UseNotifications { get; set; }

    public bool NotificationsEnabled
    {
        get
        {
            return UseDeadline && UseNotifications;
        }
    }

    public ObservableCollection<NotificationViewModel> Notifications { get; set; } = [];

    public EditableChecklistViewModel(Checklist checklist)
    {
        _checklist = checklist;
        Notifications = _checklist.Notifications?.Select(x => new NotificationViewModel(x)).ToObservableCollection();
    }
}