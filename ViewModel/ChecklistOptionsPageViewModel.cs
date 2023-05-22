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
    public class ChecklistOptionsPageViewModel : ViewModel
    {
        private ChecklistContext _checklistContext;
        private NavigationService _navigationService;

        private Checklist _checklist;
        private Checklist _unchangedChecklist;

        #region Properties

        public int Id { get; set; }

        public string Name { get { return _checklist != null ? _checklist.Name : "Loading"; } set { if (_checklist != null) _checklist.Name = value; } }
        public bool UseDeadline { get { return _checklist != null ? _checklist.UseDeadline : false; } set { if (_checklist != null) _checklist.UseDeadline = value; } }
        public DateTime Deadline { get { return _checklist != null ? _checklist.Deadline : DateTime.Now; } set { if (_checklist != null) _checklist.Deadline = value; } }

        #endregion

        #region Commands

        public ICommand BackCommand { get; set; }

        public ICommand SaveCommand { get; set; }

        public ICommand DeleteCommand { get; set; }

        #endregion

        public ChecklistOptionsPageViewModel(ChecklistContext checklistContext, NavigationService navigationService)
        {
            _checklistContext = checklistContext;
            _navigationService = navigationService;

            BackCommand = new Command(Back);
            SaveCommand = new Command(Save);
            DeleteCommand = new Command(Delete);
        }

        #region Methods

        private void Back()
        {
            _checklistContext.DiscardChanges();
            _navigationService.NavigateTo(NavigationService.NavigationTarget.Back);
        }

        private void Save()
        {
            try
            {
                _ = _checklistContext.UpdateChecklist(_checklist);
                _navigationService.NavigateTo(NavigationService.NavigationTarget.Back);
            }
            catch
            {
                //  TODO: Notify user of error
            }
        }

        private void Delete()
        {
            try
            {
                _checklistContext.DeleteChecklist(_checklist);
                _navigationService.NavigateTo(NavigationService.NavigationTarget.Home);
            }
            catch
            {
                //  TODO: Notify user of error
            }
        }

        public async void RetrieveChecklist(object sender, EventArgs e)
        {
            _checklist = await _checklistContext.GetChecklist(Id);
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(UseDeadline));
            OnPropertyChanged(nameof(Deadline));
        }

        #endregion
    }
}
