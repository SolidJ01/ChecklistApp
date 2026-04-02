using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text.Json;
using System.Windows.Input;
using ChecklistApp.Data;
using ChecklistApp.Model;
using ChecklistApp.Model.Remote;
using ChecklistApp.Services;
using CommunityToolkit.Maui.Core.Extensions;
using Version = ChecklistApp.Model.Remote.Version;

namespace ChecklistApp.ViewModel;

public class SettingsPopupViewModel : ViewModel
{
    public enum RefreshState
    {
        Checking, 
        Idle, 
        UpdateAvailable
    }

    private static readonly int s_maxUpdateChecksToNotify = 1;
    
    private readonly IPreferences _preferences;
    private readonly IVersionTracking _versionTracking;
    private readonly HttpClient _httpClient;
    private readonly ChecklistContext _context;
    private readonly ToastService _toastService;
    private readonly UpdateCheckerService _updateService;

    private bool _notificationsEnabled = false;
    private Release _release;
    private int nUpdateChecks = 0;

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

    public string ReleaseInfo { get; set; }
    public string UpdateTitle { get; set; } = "";
    public string UpdateSubtitle { get; set; } = "";
    public RefreshState UpdateState { get; set; } = RefreshState.Idle;
    
    public ICommand UpdateCommand { get; set; }

    public SettingsPopupViewModel(IPreferences preferences, 
                                  IVersionTracking versionTracking, 
                                  HttpClient httpClient, 
                                  ChecklistContext context, 
                                  ToastService toastService, 
                                  UpdateCheckerService updateService)
    {
        _preferences = preferences;
        _versionTracking = versionTracking;
        _httpClient = httpClient;
        _context = context;
        _toastService = toastService;
        _updateService = updateService;

        UpdateCommand = new Command(Update);

        ReleaseInfo =
            $"Version {_versionTracking.CurrentVersion}   Released {Environment.GetEnvironmentVariable(StringHelper.S_EnvironmentReleaseDate)}";
        
        GetPreferences();
        GetNotificationDefaults();
        Update();
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
            _toastService.QueueToast($"Error: {ex.Message}");
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
            _toastService.QueueToast($"Error: {ex.Message}");
        }
    }

    private async void Update()
    {
        switch (UpdateState)
        {
            case RefreshState.Idle:
                await RetrieveLatestRelease();
                break;
            case RefreshState.Checking:
                return;
            case RefreshState.UpdateAvailable:
                await SendToDownload();
                break;
        }
    }

    private async Task RetrieveLatestRelease()
    {
        UpdateState = RefreshState.Checking;
        UpdateTitle = "Checking for Update...";
        UpdateSubtitle = "";
        OnPropertyChanged(nameof(UpdateState));
        OnPropertyChanged(nameof(UpdateTitle));
        OnPropertyChanged(nameof(UpdateSubtitle));
        nUpdateChecks++;

        try
        {
            _release = await _updateService.GetLatestRelease();
        }
        catch (Exception e)
        {
            UpdateTitle = "Release API Unavailable";
            UpdateSubtitle = "";
            UpdateState = RefreshState.Idle;
            OnPropertyChanged(nameof(UpdateTitle));
            OnPropertyChanged(nameof(UpdateSubtitle));
            OnPropertyChanged(nameof(UpdateState));
            return;
        }
        
        if (_release is not null && _release.Version > Version.Parse(_versionTracking.CurrentVersion))
        {
            UpdateTitle = $"{_release.Version} Available";
            UpdateSubtitle = _release.Published.ToString("O");
            OnPropertyChanged(nameof(UpdateTitle));
            OnPropertyChanged(nameof(UpdateSubtitle));
            
            
            if (ShouldNotifyUpdate())
            {
                _toastService.QueueToast($"Update {_release.Version} available!");
                _preferences.Set(StringHelper.S_PreferenceLastReleaseNotified, _release.Version.ToString());
            }
            
            UpdateState = RefreshState.UpdateAvailable;
            OnPropertyChanged(nameof(UpdateState));
            return;
        }
        
        UpdateTitle = "Up To Date";
        UpdateSubtitle = "";
        UpdateState = RefreshState.Idle;
        OnPropertyChanged(nameof(UpdateTitle));
        OnPropertyChanged(nameof(UpdateSubtitle));
        OnPropertyChanged(nameof(UpdateState));
    }

    private async Task SendToDownload()
    {
        try
        {
            Uri uri = new Uri("https://www.fastcode.se/software/checklist");
            await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            _toastService.QueueToast($"Error: {ex.Message}");
        }
    }

    private bool ShouldNotifyUpdate()
    {
        if (!_preferences.ContainsKey(StringHelper.S_PreferenceLastReleaseNotified))
            return nUpdateChecks <= s_maxUpdateChecksToNotify;

        try
        {
            Version lastNotifiedVersion =
                Version.Parse(_preferences.Get(StringHelper.S_PreferenceLastReleaseNotified, string.Empty));
            return lastNotifiedVersion < _release.Version && nUpdateChecks <= s_maxUpdateChecksToNotify;
        }
        catch (Exception e)
        {
            _toastService.QueueToast($"Error: {e.Message}");
            return false;
        }
    }
}