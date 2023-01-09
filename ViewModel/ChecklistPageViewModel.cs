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
    [QueryProperty(nameof(Id), "id")]
    public class ChecklistPageViewModel : ViewModel
    {
        private ChecklistContext _checklistContext;
        private NavigationService _navigationService;

        private Checklist _checklist;

        #region Properties

        public int Id { get; set; }

        public Checklist Checklist { get { return _checklist; } set { _checklist = value; } }

        public ChecklistCardViewModel ChecklistCard { get; set; }

        public ObservableCollection<Item> Items { get; set; } = new ObservableCollection<Item>();

        #endregion

        #region Commands

        public ICommand BackCommand { get; set; }

        public ICommand OptionsCommand { get; set; }

        public ICommand CreateNewCommand { get; set; }

        #endregion

        public ChecklistPageViewModel(ChecklistContext checklistContext, NavigationService navigationService)
        {
            _checklistContext = checklistContext;
            _navigationService = navigationService;

            BackCommand = new Command(Back);
            OptionsCommand = new Command(Options);
            CreateNewCommand = new Command(CreateNew);
        }

        #region Methods

        private void Back()
        {
            _navigationService.NavigateTo(NavigationService.NavigationTarget.Back);
        }

        private void Options()
        {
            _navigationService.NavigateTo(NavigationService.NavigationTarget.ChecklistOptions, Id);
        }

        private void CreateNew()
        {
            _navigationService.NavigateTo(NavigationService.NavigationTarget.CreateItem, Id);
        }

        public async void RetrieveChecklist(object sender, EventArgs e)
        {
            _checklist = await _checklistContext.GetChecklist(Id);
            OnPropertyChanged(nameof(Checklist));

            ChecklistCard = new ChecklistCardViewModel(_checklist);
            OnPropertyChanged(nameof(ChecklistCard));

            Items = new ObservableCollection<Item>(_checklist.Items);
            OnPropertyChanged(nameof(Items));
        }

        #endregion
    }
}
