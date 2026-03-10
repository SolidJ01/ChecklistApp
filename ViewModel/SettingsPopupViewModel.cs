using System.Collections.ObjectModel;
using ChecklistApp.Data;
using CommunityToolkit.Maui.Core.Extensions;

namespace ChecklistApp.ViewModel;

public class SettingsPopupViewModel : ViewModel
{
    private static readonly string S_PreferenceNotificationsEnabled = "NotificationsEnabled";
    
    private IPreferences _preferences;
    private ChecklistContext _context;

    private bool _notificationsEnabled = false;

    public bool NotificationsEnabled
    {
        get { return _notificationsEnabled; }
        set
        {
            _notificationsEnabled = value;
            OnPropertyChanged(nameof(NotificationsEnabled));
            _preferences.Set(S_PreferenceNotificationsEnabled, value);
        }
    }
    public ObservableCollection<NotificationViewModel> Notifications { get; set; } = [];

    public SettingsPopupViewModel(IPreferences preferences, ChecklistContext context)
    {
        _preferences = preferences;
        _context = context;
        
        GetPreferences();
        GetNotificationDefaults();
    }

    private void GetPreferences()
    {
        if (!_preferences.ContainsKey(S_PreferenceNotificationsEnabled))
        {
            _preferences.Set(S_PreferenceNotificationsEnabled, false);
            return;
        }
        NotificationsEnabled = _preferences.Get<bool>(S_PreferenceNotificationsEnabled, false);
        OnPropertyChanged(nameof(NotificationsEnabled));
    }

    private void GetNotificationDefaults()
    {
        Notifications = _context.GetNotificationDefaults().Result.Select(x => new NotificationViewModel(x)).ToObservableCollection();
        // TODO: listen for Notifications.CollectionChanged and NotificationViewModel.PropertyChanged to update the database
    }
}