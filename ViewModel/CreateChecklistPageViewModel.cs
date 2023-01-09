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
    public class CreateChecklistPageViewModel
    {
        private ChecklistContext _checklistContext;
        private NavigationService _navigationService;
        private Checklist _checklist;

        #region Properties

        public string Name { get { return _checklist.Name; } set { _checklist.Name = value; } }
        public bool UseDeadline { get { return _checklist.UseDeadline; } set { _checklist.UseDeadline = value; } }

        #endregion

        #region Commands

        public ICommand BackCommand { get; set; }

        #endregion

        public CreateChecklistPageViewModel(ChecklistContext checklistContext, NavigationService navigationService)
        {
            _checklistContext = checklistContext;
            _navigationService = navigationService;
            _checklist = new Checklist();

            BackCommand = new Command(Back);
        }

        private void Back()
        {
            _navigationService.NavigateTo(NavigationService.NavigationTarget.Back);
        }
    }
}
