using ChecklistApp.Data;
using ChecklistApp.Model;
using ChecklistApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ChecklistApp.ViewModel
{
    public class CreateChecklistPopupViewModel : ViewModel
    {
        public event EventHandler ChecklistAdded;
        private ChecklistContext _checklistContext;
        private INotificationManagerService _notificationManagerService;
        private Checklist _checklist;
        private bool _notificationsEnabled;

        #region Properties

        public string Name { get { return _checklist.Name; } set { _checklist.Name = value; } }
        public bool UseDeadline { get { return _checklist.UseDeadline; } set { _checklist.UseDeadline = value; } }
        public DateTime Deadline { get { return _checklist.Deadline; } set { _checklist.Deadline = value; } }
        public Checklist.ChecklistColor Color { get { return _checklist.Color; } set { _checklist.Color = value; } }

        public bool NotificationsEnabled
        {
            get { return _notificationsEnabled; }
            set
            {
                _notificationsEnabled = value; 
                OnPropertyChanged(nameof(NotificationsEnabled));
            }
        }
        public ObservableCollection<NotificationViewModel> Notifications { get; set; }

        #endregion

        #region Commands

        public ICommand CancelCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand ImportCommand { get; set; }

        #endregion

        public CreateChecklistPopupViewModel(ChecklistContext checklistContext, INotificationManagerService notificationManagerService)
        {
            _checklistContext = checklistContext;
            _notificationManagerService = notificationManagerService;
            ResetChecklist();

            CancelCommand = new Command(Cancel);
            SaveCommand = new Command<Action>(Save);
            ImportCommand = new Command<Action>(Import);
        }

        private void ResetChecklist()
        {
            _checklist = new Checklist { Deadline = DateTime.Now };
            NotificationsEnabled = false;
            Notifications = [];
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(UseDeadline));
            OnPropertyChanged(nameof(Deadline));
            OnPropertyChanged(nameof(Color));
            OnPropertyChanged(nameof(NotificationsEnabled));
            OnPropertyChanged(nameof(Notifications));
        }

        private void Cancel()
        {
            ResetChecklist();
        }

        private void Save(Action callback = null)
        {
            if (Name is null)
                return;
            
            try
            {
                _checklist.Name = StringHelper.FormatItemName(Name);
                _checklist.Notifications = UseDeadline && NotificationsEnabled ? Notifications.Select(x => x.Notification).ToList() : [];
                foreach (Notification notification in _checklist.Notifications) 
                    notification.Checklist = _checklist;
                _checklistContext.CreateChecklist(_checklist);

                foreach (Notification notification in _checklist.Notifications)
                {
                    _notificationManagerService.SendNotification
                    (
                        notification.Id, 
                        StringHelper.GenerateNotificationTitle(notification), 
                        StringHelper.GenerateNotificationMessage(notification), 
                        _checklist.Deadline.Subtract(notification.Value)
                    );
                }
                
                ChecklistAdded?.Invoke(this, EventArgs.Empty);
                callback?.Invoke();
            }
            catch
            {
                //  TODO: Notify user something went wrong
            }
        }

        private async void Import(Action callback = null)
        {
            try
            {
                PickOptions options = new PickOptions
                {
                    PickerTitle = "Select a .json file",
                    FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                    {
                        { DevicePlatform.Android, ["application/json"] },
                    })
                };
                var result = await FilePicker.Default.PickAsync(options);
                if (result != null)
                {
                    var stream = await result.OpenReadAsync();
                    try
                    {
                        Checklist checklist = JsonSerializer.Deserialize<Checklist>(stream);
                        checklist.Id = 0;
                        foreach (Item item in checklist.Items)
                        {
                            item.Id = 0;
                        }

                        _checklistContext.CreateChecklist(checklist);
                    }
                    catch (JsonException e)
                    {
                        stream = await result.OpenReadAsync();
                        
                        List<Checklist> checklists = JsonSerializer.Deserialize<List<Checklist>>(stream);
                        foreach (Checklist checklist in checklists)
                        {
                            checklist.Id = 0;
                            foreach (Item item in checklist.Items)
                            {
                                item.Id = 0;
                                item.Checklist = checklist;
                            }
                            _checklistContext.CreateChecklist(checklist);
                        }
                        //_checklistContext.CreateChecklists(checklists);
                    }
                    ChecklistAdded?.Invoke(this, EventArgs.Empty);
                    callback?.Invoke();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
