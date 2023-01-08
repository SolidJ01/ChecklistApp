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
        private ObservableCollection<ChecklistCardViewModel> _checklistViewModels;
        public ObservableCollection<ChecklistCardViewModel> Checklists { get { return _checklistViewModels; } set { _checklistViewModels = value; } }

        #region Commands

        public ICommand CreateNewCommand { get; set; }

        #endregion

        public MainPageViewModel(ChecklistContext checklistContext, NavigationService navigationService)
        {
            _checklistContext = checklistContext;
            _navigationService = navigationService;

            _checklists = new ObservableCollection<Checklist>(_checklistContext.GetChecklists());
            _checklistViewModels = new ObservableCollection<ChecklistCardViewModel>();
            foreach (Checklist checklist in _checklists)
            {
                _checklistViewModels.Add(new ChecklistCardViewModel(checklist));
            }

            CreateNewCommand = new Command(CreateNew);
        }

        #region Methods

        private void CreateNew()
        {
            _navigationService.NavigateTo(NavigationService.NavigationTarget.CreateChecklist);
        }

        #endregion
    }
}
