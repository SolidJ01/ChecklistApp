using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using ChecklistApp.Data;
using ChecklistApp.Model;
using ChecklistApp.Services;
using CommunityToolkit.Maui.Core.Extensions;

namespace ChecklistApp.ViewModel;

public class SettingsPopupViewModel : ViewModel
{
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
            _preferences.Set(StringHelper.S_PreferenceNotificationsEnabled, value);
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
        if (!_preferences.ContainsKey(StringHelper.S_PreferenceNotificationsEnabled))
        {
            _preferences.Set(StringHelper.S_PreferenceNotificationsEnabled, false);
            return;
        }
        NotificationsEnabled = _preferences.Get<bool>(StringHelper.S_PreferenceNotificationsEnabled, false);
        OnPropertyChanged(nameof(NotificationsEnabled));
    }

    private void GetNotificationDefaults()
    {
        Notifications = _context.GetNotificationDefaults().Result.Select(x => new NotificationViewModel(x)).ToObservableCollection();
        Notifications.CollectionChanged += NotificationsCollectionChanged;
        foreach (var notification in Notifications)
            notification.PropertyChanged += NotificationsItemChanged;
    }

    private async void NotificationsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        try
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems is null)
                        return;
                    foreach (NotificationViewModel notification in e.NewItems)
                    {
                        await _context.CreateNotification(notification.Notification);
                        notification.PropertyChanged += NotificationsItemChanged;
                    }

                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldItems is null)
                        return;
                    foreach (NotificationViewModel notification in e.OldItems)
                    {
                        notification.PropertyChanged -= NotificationsItemChanged;
                        await _context.DeleteNotification(notification.Notification);
                    }

                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            //  TODO: toast
        }
    }

    private async void NotificationsItemChanged(object sender, PropertyChangedEventArgs e)
    {
        try
        {
            if (sender is not NotificationViewModel notification)
                return;
            await _context.UpdateNotification(notification.Notification);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            //  TODO: toast
        }
    }
}