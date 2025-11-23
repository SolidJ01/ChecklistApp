using ChecklistApp.Data;
using ChecklistApp.Model;
using ChecklistApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ChecklistApp.ViewModel
{
    public class MainPageViewModel : ViewModel
    {
        private ChecklistContext _checklistContext;
        private NavigationService _navigationService;

        public ObservableCollection<ChecklistCardViewModel> Checklists { get; set; } = [];

        #region Commands

        public ICommand CreateNewCommand { get; set; }

        public ICommand GoToChecklistCommand { get; set; }

        #endregion

        public MainPageViewModel(ChecklistContext checklistContext, NavigationService navigationService)
        {
            _checklistContext = checklistContext;
            _navigationService = navigationService;

            CreateNewCommand = new Command(CreateNew);
            GoToChecklistCommand = new Command<int>(GoToChecklist);
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
                    Checklists.Add(new ChecklistCardViewModel(checklist));
                }

                return;
            }
            
            List<ChecklistCardViewModel> removedChecklists = Checklists.Where(x => !checklists.Any(y => y.Id == x.Id)).ToList();
            List<Checklist> addedChecklists = checklists.Where(x => !Checklists.Any(y => y.Id == x.Id)).ToList();
            // List<Checklist> editedChecklists = checklists.Where(x => Checklists.Any(y => y.Id == x.Id) 
            //                                                               && !Checklists.First(y => y.Id == x.Id).ModelEquals(x))
            //                                                               .ToList();
            foreach (ChecklistCardViewModel checklist in removedChecklists)
                Checklists.Remove(checklist);
            foreach (Checklist checklist in addedChecklists)
                Checklists.Insert(checklists.IndexOf(checklist), new ChecklistCardViewModel(checklist));
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

        #endregion
    }
}
