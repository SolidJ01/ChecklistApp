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

        private ObservableCollection<Checklist> _checklists;
        public ObservableCollection<ChecklistCardViewModel> Checklists { get; set; }

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
            _checklists = new ObservableCollection<Checklist>(checklists);
            Checklists = new ObservableCollection<ChecklistCardViewModel>();
            foreach (Checklist checklist in _checklists)
            {
                Checklists.Add(new ChecklistCardViewModel(checklist));
            }
            OnPropertyChanged(nameof(Checklists));
        }

        private void GoToChecklist(int checklist)
        {
            _navigationService.NavigateTo(NavigationService.NavigationTarget.Checklist, checklist);
        }

        #endregion
    }
}
