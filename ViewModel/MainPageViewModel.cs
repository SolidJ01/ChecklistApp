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
using CommunityToolkit.Maui.Storage;

namespace ChecklistApp.ViewModel
{
    public class MainPageViewModel : ViewModel
    {
        private ChecklistContext _checklistContext;
        private NavigationService _navigationService;
        private IFileSaver _fileSaver;
        private INotificationManagerService _notificationService;

        public ObservableCollection<SelectableChecklistCardViewModel> Checklists { get; set; } = [];
        public Checklist ChecklistEntry { get; set; } = new Checklist { Deadline = DateTime.Now };

        #region Commands

        public ICommand CreateNewCommand { get; set; }
        public ICommand GoToChecklistCommand { get; set; }
        public ICommand ExportChecklistsCommand { get; set; }
        public ICommand SaveNewChecklistCommand { get; set; }
        public ICommand CancelNewChecklistCommand { get; set; }
        public ICommand ImportNewChecklistCommand { get; set; }
        public ICommand Test_SendNotificationCommand { get; set; }
        public ICommand Test_CancelNotificationCommand { get; set; }

        #endregion

        public MainPageViewModel(ChecklistContext checklistContext, NavigationService navigationService, IFileSaver  fileSaver, INotificationManagerService notificationService)
        {
            _checklistContext = checklistContext;
            _navigationService = navigationService;
            _fileSaver = fileSaver;
            _notificationService = notificationService;

            CreateNewCommand = new Command(CreateNew);
            GoToChecklistCommand = new Command<int>(GoToChecklist);
            ExportChecklistsCommand = new Command<Action>(ExportChecklists);
            SaveNewChecklistCommand = new Command<Action>(SaveNewChecklist);
            CancelNewChecklistCommand = new Command(CancelNewChecklist);
            ImportNewChecklistCommand = new Command<Action>(ImportnewChecklist);
            
            Test_SendNotificationCommand = new Command(async () =>
            {
                PermissionStatus status = await Permissions.RequestAsync<NotificationPermission>();
                if (status != PermissionStatus.Granted)
                    return;
                _notificationService.SendNotification("Title", "Message", DateTime.Now.AddSeconds(10));
            });
            Test_CancelNotificationCommand = new Command(() =>
            {
                _notificationService.CancelNotification();
            });
        }

        #region Methods

        private void CreateNew()
        {
            _navigationService.NavigateTo(NavigationService.NavigationTarget.CreateChecklist);
        }

        public async void ReloadList(object sender, EventArgs e)
        {
            List<Checklist> checklists = await _checklistContext.GetChecklists();
            
            if (Checklists.Count == 0)
            {
                foreach (Checklist checklist in checklists)
                {
                    Checklists.Add(new SelectableChecklistCardViewModel(checklist));
                }

                return;
            }
            
            List<SelectableChecklistCardViewModel> removedChecklists = Checklists.Where(x => !checklists.Any(y => y.Id == x.Id)).ToList();
            List<Checklist> addedChecklists = checklists.Where(x => !Checklists.Any(y => y.Id == x.Id)).ToList();
            // List<Checklist> editedChecklists = checklists.Where(x => Checklists.Any(y => y.Id == x.Id) 
            //                                                               && !Checklists.First(y => y.Id == x.Id).ModelEquals(x))
            //                                                               .ToList();
            foreach (SelectableChecklistCardViewModel checklist in removedChecklists)
                Checklists.Remove(checklist);
            foreach (Checklist checklist in addedChecklists)
                Checklists.Insert(checklists.IndexOf(checklist), new SelectableChecklistCardViewModel(checklist));
            // foreach (Checklist checklist in editedChecklists)
            //     Checklists[Checklists.IndexOf(Checklists.First(x => x.Id == checklist.Id))].Update(checklist);
            foreach (ChecklistCardViewModel checklist in Checklists)
            {
                checklist.Update();
            }
        }

        private void GoToChecklist(int checklist)
        {
            _navigationService.NavigateTo(NavigationService.NavigationTarget.Checklist, checklist);
        }

        private async void ExportChecklists(Action callback = null)
        {
            List<Checklist> checklists = [];
            foreach (var checklistViewModel in Checklists.Where(x => x.Selected))
            {
                Checklist checklist = _checklistContext.GetChecklist(checklistViewModel.Id).Result;
                foreach (var item in checklist.Items)
                {
                    item.Checklist = null;
                }
                checklists.Add(checklist);
            }
            
            var stream = new MemoryStream(Encoding.Default.GetBytes(JsonSerializer.Serialize(checklists)));
            var fileSaverResult = await _fileSaver.SaveAsync("checklists.json", stream);
            
            if (fileSaverResult.IsSuccessful)
                callback?.Invoke();
        }

        private void SaveNewChecklist(Action callback = null)
        {
            if (ChecklistEntry.Name is null)
                return;
            
            ChecklistEntry.Name = StringHelper.FormatItemName(ChecklistEntry.Name);
            _checklistContext.CreateChecklist(ChecklistEntry);
            ReloadList(this, new());
            ChecklistEntry = new Checklist { Deadline = DateTime.Now };
            OnPropertyChanged(nameof(ChecklistEntry));
            callback?.Invoke();
        }

        private void CancelNewChecklist()
        {
            ChecklistEntry =  new Checklist { Deadline = DateTime.Now };
            OnPropertyChanged(nameof(ChecklistEntry));
        }

        private async void ImportnewChecklist(Action callback = null)
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
                    ReloadList(this, EventArgs.Empty);
                    callback?.Invoke();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        #endregion
    }
}
