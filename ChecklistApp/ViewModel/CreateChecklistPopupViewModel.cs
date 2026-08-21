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
using CommunityToolkit.Maui.Core.Extensions;

namespace ChecklistApp.ViewModel
{
    public class CreateChecklistPopupViewModel : ViewModel
    {
        public event EventHandler ChecklistAdded;
        private ChecklistContext _checklistContext;
        private Checklist _checklist;
        private INotificationManagerService _notificationManagerService;
        private IPreferences _preferences;
        private ToastService _toastService;
        private ColorService _colorService;
        
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
        
        public ObservableCollection<SelectableColorViewModel> SelectableColors { get; set; }

        #endregion

        #region Commands

        public ICommand CancelCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand ImportCommand { get; set; }
        public ICommand SetColorCommand { get; set; }
        public ICommand SetCustomColorCommand { get; set; }
        public ICommand EditCustomColorCommand { get; set; }
        public ICommand AddNewColorCommand { get; set; }

        #endregion

        public CreateChecklistPopupViewModel(ChecklistContext checklistContext, INotificationManagerService notificationManagerService, IPreferences preferences, ToastService toastService, ColorService colorService)
        {
            _checklistContext = checklistContext;
            _notificationManagerService = notificationManagerService;
            _preferences = preferences;
            _toastService = toastService;
            _colorService = colorService;
            ResetChecklist();

            CancelCommand = new Command(Cancel);
            SaveCommand = new Command<Action>(Save);
            ImportCommand = new Command<Action>(Import);

            SetColorCommand = new Command<Checklist.ChecklistColor>(SetColor);
            SetCustomColorCommand = new Command<int>(SetCustomColor);
            EditCustomColorCommand = new Command<int>(EditCustomColor);
            AddNewColorCommand = new Command(AddNewColor);

            ObservableCollection<SelectableColorViewModel> selectableColors = [];
            foreach (Checklist.ChecklistColor color in Enum.GetValues<Checklist.ChecklistColor>())
            {
                if (color == Checklist.ChecklistColor.Custom)
                    continue;
                selectableColors.Add(new SelectableColorViewModel(selectedIcon:"")
                {
                    Color = color, 
                    Selected = color == Color, 
                    Command = SetColorCommand
                });
            }

            List<ChecklistColor> customColors = _checklistContext.GetColorsAsync().Result;
            foreach (ChecklistColor color in customColors)
            {
                selectableColors.Add(new SelectableColorViewModel(selectedIcon:"", smallIcon:"", selectedSmallIcon:"")
                {
                    Color = Checklist.ChecklistColor.Custom,
                    CustomColorId =  color.Id,
                    Selected = Color == Checklist.ChecklistColor.Custom && _checklist.CustomColorId == color.Id,
                    Command = SetCustomColorCommand,
                    SelectedCommand = EditCustomColorCommand
                });
            }

            selectableColors.Add(new SelectableColorViewModel(icon:"", selectedIcon:"")
            {
                Color = Checklist.ChecklistColor.Grey,
                Selected = false,
                Command = AddNewColorCommand
            });
            SelectableColors = selectableColors;
            OnPropertyChanged(nameof(SelectableColors));
        }
        
        #region ChecklistMethods

        public void ResetChecklist()
        {
            _checklist = new Checklist { Deadline = DateTime.Now };
            NotificationsEnabled = _preferences.Get(StringHelper.S_PreferenceNotificationsEnabled, false);
            Notifications = NotificationsEnabled
                ? _checklistContext.GetNotificationDefaults().Result.Select(x => new NotificationViewModel(x))
                    .ToObservableCollection()
                : [];
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
            SetColor(Checklist.ChecklistColor.Grey);
        }

        private async void Save(Action callback = null)
        {
            if (string.IsNullOrWhiteSpace(Name))
                return;
            
            try
            {
                _checklist.Name = StringHelper.FormatItemName(Name);
                _checklist.Notifications = UseDeadline && NotificationsEnabled ? Notifications.Select(x => new Notification { Checklist = _checklist, Value = x.Notification.Value}).ToList() : [];
                foreach (Notification notification in _checklist.Notifications) 
                    notification.Checklist = _checklist;
                await _checklistContext.CreateChecklist(_checklist);

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
            catch (Exception e)
            {
                _toastService.QueueToast($"Error: {e?.InnerException?.Message}");
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
                        _toastService.QueueToast("Successfully imported checklist");
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
                        _toastService.QueueToast($"Successfully imported {checklists.Count} checklists");
                        //_checklistContext.CreateChecklists(checklists);
                    }
                    ChecklistAdded?.Invoke(this, EventArgs.Empty);
                    callback?.Invoke();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                _toastService.QueueToast(e.Message);
            }
        }
        
        #endregion
        
        #region ColorMethods

        private void SetColor(Checklist.ChecklistColor color)
        {
            _checklist.Color = color;
            _checklist.CustomColorId = null;
            foreach (var selectableColor in SelectableColors)
            {
                selectableColor.Selected = selectableColor.Color == color && selectableColor.Command == SetColorCommand;
            }
        }

        private void SetCustomColor(int colorId)
        {
            _checklist.Color = Checklist.ChecklistColor.Custom;
            _checklist.CustomColorId = colorId;
            foreach (var selectableColor in SelectableColors)
            {
                selectableColor.Selected = selectableColor.Color == Checklist.ChecklistColor.Custom && selectableColor.CustomColorId == colorId;
            }
        }

        private void AddNewColor()
        {
            _colorService.RequestColourCreation(OnNewColorAdded);
        }

        private void OnNewColorAdded(int id)
        {
            ChecklistColor color = _checklistContext.GetColorAsync(id).Result;
            
            SelectableColors.Insert(SelectableColors.Count - 1, new SelectableColorViewModel(selectedIcon:"", smallIcon:"", selectedSmallIcon:"")
            {
                Color = Checklist.ChecklistColor.Custom, 
                CustomColorId = id,
                Selected = true,
                Command = SetCustomColorCommand,
                SelectedCommand = EditCustomColorCommand
            });
            
            SetCustomColor(id);
        }

        private void OnColorRemoved(int id)
        {
            SelectableColorViewModel? color = SelectableColors.FirstOrDefault(x => x.CustomColorId == id);
            if (color is null)
                return;
            if (color.Selected)
            {
                SetColor(Checklist.ChecklistColor.Grey);
            }
            SelectableColors.Remove(color);
        }

        private void EditCustomColor(int colorId)
        {
            _colorService.RequestColourEditing(colorId, OnColorsChanged);
        }

        private void OnColorsChanged()
        {
            List<ChecklistColor> colors = _checklistContext.GetColorsAsync().Result;
            foreach (var checklistColor in colors)
            {
                if (SelectableColors.All(x => x.CustomColorId != checklistColor.Id))
                {
                    OnNewColorAdded(checklistColor.Id);
                }
            }

            foreach (var selectableColor in SelectableColors.Where(x => x.Color.Equals(Checklist.ChecklistColor.Custom)).ToList())
            {
                if (colors.All(x => x.Id != selectableColor.CustomColorId) && selectableColor.CustomColorId is not null)
                {
                    OnColorRemoved((int)selectableColor.CustomColorId);
                }
            }
        }
        
        #endregion
    }
}
