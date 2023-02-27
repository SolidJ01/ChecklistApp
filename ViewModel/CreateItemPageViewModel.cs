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
    public class CreateItemPageViewModel : ViewModel
    {
        private ChecklistContext _checklistContext;
        private NavigationService _navigationService;

        private Checklist _checklist;

        #region Properties

        public int Id { get; set; }

        public ObservableCollection<Item> Items { get; set; } = new ObservableCollection<Item>();

        #endregion

        #region Commands

        public ICommand AddNewCommand { get; set; }

        public ICommand RemoveCommand { get; set; }

        public ICommand SaveCommand { get; set; }

        public ICommand BackCommand { get; set; }

        #endregion

        public CreateItemPageViewModel(ChecklistContext checklistContext, NavigationService navigationService)
        {
            _checklistContext = checklistContext;
            _navigationService = navigationService;

            AddNewCommand = new Command(AddNew);
            RemoveCommand = new Command<Item>(Remove);
            SaveCommand = new Command(Save);
            BackCommand = new Command(Back);
        }

        #region Methods


        private void AddNew()
        {
            Items.Add(new Item { Checklist = _checklist });
        }

        private void Remove(Item item)
        {
            if (item is null)
                return;

            try
            {
                Items.Remove(item);
            }
            catch
            {
                //  TODO: Notify user of error
            }
        }

        private void Save()
        {
            try
            {
                _checklistContext.CreateItems(Items.ToList());
                _navigationService.NavigateTo(NavigationService.NavigationTarget.Back);
            }
            catch
            {
                //  TODO: Notify user of error
            }
        }

        private void Back()
        {
            _navigationService.NavigateTo(NavigationService.NavigationTarget.Back);
        }

        public async void RetrieveChecklist(object sender, EventArgs e)
        {
            _checklist = await _checklistContext.GetChecklist(Id);
            Items.Clear();
            Items.Add(new Item { Checklist = _checklist });
        }

        #endregion
    }
}
