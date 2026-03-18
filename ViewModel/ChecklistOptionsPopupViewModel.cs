using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json;
using System.Windows.Input;
using ChecklistApp.Data;
using ChecklistApp.Model;
using ChecklistApp.Services;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Storage;

namespace ChecklistApp.ViewModel;

public class ChecklistOptionsPopupViewModel : ViewModel
{
    public event EventHandler ChangesSaved;
    private readonly ChecklistContext _context;
    private readonly IFileSaver _fileSaver;
    private readonly INotificationManagerService _notificationService;
    private readonly NavigationService _navigationService;
    private readonly ToastService _toastService;

    private Checklist _checklist;
    private Checklist _checklistEdit = new Checklist { Deadline = DateTime.Now };
    private bool _notificationsEnabled;

    public string Name { get { return _checklistEdit.Name; } set { _checklistEdit.Name = value; OnPropertyChanged(); } }
    public bool UseDeadline { get { return _checklistEdit.UseDeadline; } set { _checklistEdit.UseDeadline = value; OnPropertyChanged(); } }
    public DateTime Deadline { get { return _checklistEdit.Deadline; } set { _checklistEdit.Deadline = value; OnPropertyChanged(); } }
    public Checklist.ChecklistColor Color { get { return _checklistEdit.Color; } set { _checklistEdit.Color = value; OnPropertyChanged(); } }
    public bool NotificationsEnabled
    {
        get { return _notificationsEnabled; }
        set
        {
            _notificationsEnabled = value; 
            OnPropertyChanged(nameof(NotificationsEnabled));
        }
    }

    public ObservableCollection<NotificationViewModel> Notifications { get; set; } = [];
    
    public ICommand ExportChecklistCommand { get; set; }
    public ICommand CancelChecklistEditCommand { get; set; }
    public ICommand SaveChecklistEditCommand { get; set; }
    public ICommand DeleteChecklistCommand { get; set; }

    public ChecklistOptionsPopupViewModel(ChecklistContext context, IFileSaver fileSaver, INotificationManagerService notificationService, NavigationService navigationService, ToastService toastService)
    {
        _context = context;
        _fileSaver = fileSaver;
        _notificationService = notificationService;
        _navigationService = navigationService;
        _toastService = toastService;
            
        ExportChecklistCommand = new Command(ExportChecklist);
        CancelChecklistEditCommand = new Command(CancelChecklistEdit);
        SaveChecklistEditCommand = new Command<Action>(SaveChecklist);
        DeleteChecklistCommand = new Command(DeleteChecklist);
    }

    public void Load(int id)
    {
        _checklist = _context.GetChecklist(id).Result;
        ResetEditEntry();
    }
    
    private async void ExportChecklist()
        {
            List<Notification> notifications = _checklist.Notifications;
            _checklist.Notifications = [];
            foreach (var item in _checklist.Items)
            {
                item.Checklist = null;
            }
            var stream = new MemoryStream(Encoding.Default.GetBytes(JsonSerializer.Serialize(_checklist)));
            var result = await _fileSaver.SaveAsync($"{_checklist.Name}.json", stream);
            foreach (var item in _checklist.Items)
            {
                item.Checklist = _checklist;
            }
            _checklist.Notifications = notifications;
            if (result.IsSuccessful)
                _toastService.QueueToast("Successfully exported checklist");
        }

        private void CancelChecklistEdit()
        {
            ResetEditEntry();
        }

        private void SaveChecklist(Action callback = null)
        {
            _checklist.Name = Name;
            _checklist.UseDeadline = UseDeadline;
            _checklist.Deadline = Deadline;
            _checklist.Color = Color;

            List<Notification> newNotifications = UseDeadline && NotificationsEnabled ? Notifications.Select(x => x.Notification).ToList() : [];
            foreach (Notification notification in _checklist.Notifications)
            {
                _notificationService.CancelNotification(notification.Id);
            }
            foreach (Notification notification in _checklist.Notifications.Where(x => !newNotifications.Contains(x)).ToList())
            {
                _checklist.Notifications.Remove(notification);
                _context.DeleteNotification(notification);
            }

            _checklist.Notifications = newNotifications;
            
            _ = _context.UpdateChecklist(_checklist);

            foreach (Notification notification in _checklist.Notifications)
            {
                _notificationService.SendNotification(notification.Id, StringHelper.GenerateNotificationTitle(notification), StringHelper.GenerateNotificationMessage(notification), _checklist.Deadline.Subtract(notification.Value));
            }
            
            ChangesSaved?.Invoke(this, EventArgs.Empty);
            callback?.Invoke();
        }

        private async void DeleteChecklist()
        {
            for (int i = _checklist.Notifications.Count - 1; i >= 0; i--)
            {
                Notification notification = _checklist.Notifications[i];
                _notificationService.CancelNotification(notification.Id);
                await _context.DeleteNotification(notification);
            }
            
            _context.DeleteChecklist(_checklist);
            await _navigationService.NavigateTo(NavigationService.NavigationTarget.Home);
            _toastService.QueueToast("Checklist deleted");
        }

        private void ResetEditEntry()
        {
            Name = _checklist.Name;
            UseDeadline = _checklist.UseDeadline;
            Deadline = _checklist.Deadline;
            Color = _checklist.Color;
            NotificationsEnabled = UseDeadline &&  _checklist.Notifications.Count > 0;
            Notifications = _checklist.Notifications.Select(x => new NotificationViewModel(x)).ToObservableCollection();
            OnPropertyChanged(nameof(Notifications));
        }
}