using ChecklistApp.Data;
using ChecklistApp.Model;
using ChecklistApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ChecklistApp.ViewModel
{
    [QueryProperty(nameof(Id), "id")]
    public class ChecklistPageViewModel : ViewModel
    {
        private ChecklistContext _checklistContext;
        private NavigationService _navigationService;

        private Checklist _checklist;

        #region Properties

        public int Id { get; set; }

        public Checklist Checklist { get { return _checklist; } set { _checklist = value; } }

        #endregion

        #region Commands

        public ICommand BackCommand { get; set; }

        #endregion

        public ChecklistPageViewModel(ChecklistContext checklistContext, NavigationService navigationService)
        {
            _checklistContext = checklistContext;
            _navigationService = navigationService;

            BackCommand = new Command(Back);
        }

        #region Methods

        public void Back()
        {
            _navigationService.NavigateTo(NavigationService.NavigationTarget.Back);
        }

        public async void RetrieveChecklist(object sender, EventArgs e)
        {
            _checklist = await _checklistContext.GetChecklist(Id);
            OnPropertyChanged(nameof(Checklist));
        }

        #endregion
    }
}
