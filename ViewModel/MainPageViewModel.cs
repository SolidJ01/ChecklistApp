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
using ChecklistApp.Controls;
using CommunityToolkit.Maui.Storage;

namespace ChecklistApp.ViewModel
{
    public class MainPageViewModel : ViewModel
    {
        private ChecklistContext _checklistContext;
        private NavigationService _navigationService;
        private IFileSaver _fileSaver;
        private INotificationManagerService _notificationService;
        private ToastService _toastService;
        private CreateChecklistPopupViewModel _createChecklistVm;
        private SettingsPopupViewModel _settingsVm;

        public ObservableCollection<SelectableChecklistViewModel> Checklists { get; set; } = [];

        public CreateChecklistPopupViewModel CreateChecklistViewModel
        {
            get => _createChecklistVm;
            set => _createChecklistVm = value;
        }

        public SettingsPopupViewModel SettingsPopupViewModel
        {
            get => _settingsVm;
            set => _settingsVm = value;
        }

        #region Commands

        public ICommand CreateNewCommand { get; set; }
        public ICommand GoToChecklistCommand { get; set; }
        public ICommand ExportChecklistsCommand { get; set; }

        #endregion

        public MainPageViewModel(ChecklistContext checklistContext, 
                                 NavigationService navigationService, 
                                 IFileSaver  fileSaver, 
                                 INotificationManagerService notificationService, 
                                 ToastService toastService,
                                 CreateChecklistPopupViewModel createChecklistVm,
                                 SettingsPopupViewModel settingsVm)
        {
            _checklistContext = checklistContext;
            _navigationService = navigationService;
            _fileSaver = fileSaver;
            _notificationService = notificationService;
            _toastService = toastService;
            _createChecklistVm = createChecklistVm;
            _createChecklistVm.ChecklistAdded += ReloadList;
            _settingsVm = settingsVm;

            GoToChecklistCommand = new Command<int>(GoToChecklist);
            ExportChecklistsCommand = new Command<Action>(ExportChecklists);
        }

        #region Methods

        public async void ReloadList(object sender, EventArgs e)
        {
            List<Checklist> checklists = await _checklistContext.GetChecklists();
            
            if (Checklists.Count == 0)
            {
                foreach (Checklist checklist in checklists)
                {
                    Checklists.Add(new SelectableChecklistViewModel(checklist));
                }

                return;
            }
            
            List<SelectableChecklistViewModel> removedChecklists = Checklists.Where(x => !checklists.Any(y => y.Id == x.Id)).ToList();
            List<Checklist> addedChecklists = checklists.Where(x => !Checklists.Any(y => y.Id == x.Id)).ToList();
            // List<Checklist> editedChecklists = checklists.Where(x => Checklists.Any(y => y.Id == x.Id) 
            //                                                               && !Checklists.First(y => y.Id == x.Id).ModelEquals(x))
            //                                                               .ToList();
            foreach (SelectableChecklistViewModel checklist in removedChecklists)
                Checklists.Remove(checklist);
            foreach (Checklist checklist in addedChecklists)
                Checklists.Insert(checklists.IndexOf(checklist), new SelectableChecklistViewModel(checklist));
            // foreach (Checklist checklist in editedChecklists)
            //     Checklists[Checklists.IndexOf(Checklists.First(x => x.Id == checklist.Id))].Update(checklist);
            foreach (ChecklistViewModel checklist in Checklists)
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
            List<Checklist> checklists = Checklists.Where(x => x.Selected).Select(x => x.Checklist).ToList();
            foreach (Checklist checklist in checklists)
            {
                checklist.Notifications = [];
                foreach (var item in checklist.Items)
                {
                    item.Checklist = null;
                }
            }
            // foreach (var checklistViewModel in Checklists.Where(x => x.Selected))
            // {
            //     Checklist checklist = _checklistContext.GetChecklist(checklistViewModel.Id).Result;
            //     foreach (var item in checklist.Items)
            //     {
            //         item.Checklist = null;
            //     }
            //     checklists.Add(checklist);
            // }
            
            var stream = new MemoryStream(Encoding.Default.GetBytes(JsonSerializer.Serialize(checklists)));
            var fileSaverResult = await _fileSaver.SaveAsync("checklists.json", stream);
            
            if (fileSaverResult.IsSuccessful)
            {
                _toastService.QueueToast("Successfully exported checklists");
                callback?.Invoke();
            }
            else
            {
                _toastService.QueueToast($"Error: {fileSaverResult.Exception?.Message}");
            }
        }

        #endregion
    }
}
