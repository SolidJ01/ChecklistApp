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

        public ObservableCollection<SelectableChecklistCardViewModel> Checklists { get; set; } = [];

        #region Commands

        public ICommand CreateNewCommand { get; set; }

        public ICommand GoToChecklistCommand { get; set; }
        
        public ICommand ExportChecklistsCommand { get; set; }

        #endregion

        public MainPageViewModel(ChecklistContext checklistContext, NavigationService navigationService, IFileSaver  fileSaver)
        {
            _checklistContext = checklistContext;
            _navigationService = navigationService;
            _fileSaver = fileSaver;

            CreateNewCommand = new Command(CreateNew);
            GoToChecklistCommand = new Command<int>(GoToChecklist);
            ExportChecklistsCommand = new Command<Action>(ExportChecklists);
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
            await _fileSaver.SaveAsync("checklists.json", stream);
            
            callback?.Invoke();
        }

        #endregion
    }
}
