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

        public ObservableCollection<Item> Items { get; set; }

        #endregion

        #region Commands

        public ICommand BackCommand { get; set; }

        public ICommand OptionsCommand { get; set; }

        public ICommand CreateNewCommand { get; set; }

        public ICommand ToggleItemCheckedCommand { get; set; }
        
        public ICommand SaveItemChangesCommand { get; set; }
        
        public ICommand DeleteItemCommand { get; set; }

        #endregion

        public ChecklistPageViewModel(ChecklistContext checklistContext, NavigationService navigationService)
        {
            _checklistContext = checklistContext;
            _navigationService = navigationService;

            BackCommand = new Command(Back);
            OptionsCommand = new Command(Options);
            CreateNewCommand = new Command(CreateNew);
            ToggleItemCheckedCommand = new Command<int>(ToggleItemChecked);
            SaveItemChangesCommand = new Command<Item>(SaveItemChanges);
            DeleteItemCommand = new Command<Item>(DeleteItem);
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

        private void ToggleItemChecked(int id)
        {
            Item item = _checklist.Items.Where(x => x.Id.Equals(id)).FirstOrDefault();
            if (item != null)
            {
                item.IsChecked = !item.IsChecked;
                _checklistContext.UpdateItem(item);
                
                ConstructItemList();
            }
        }

        private void SaveItemChanges(Item item)
        {
            _checklistContext.UpdateItem(item);
        }

        private void DeleteItem(Item item)
        {
            _checklist.Items.Remove(item);
            _checklistContext.DeleteItem(item);
            ConstructItemList();
        }

        public async void RetrieveChecklist(object sender, EventArgs e)
        {
            _checklist = await _checklistContext.GetChecklist(Id);
            foreach (var item in _checklist.Items)
                item.Checklist = _checklist;
            OnPropertyChanged(nameof(Checklist));

            ChecklistCard = new ChecklistCardViewModel(_checklist);
            OnPropertyChanged(nameof(ChecklistCard));

            ConstructItemList();
            //Items = new ObservableCollection<Item>(_checklist.Items);
            //OnPropertyChanged(nameof(Items));
            //OnPropertyChanged(nameof(UncheckedItems));
            //OnPropertyChanged(nameof(CheckedItems));
        }

        private void ConstructItemList()
        {
            Items = new ObservableCollection<Item>(_checklist.Items.OrderBy(x => x.IsChecked).ThenBy(x => x.Name));
            OnPropertyChanged(nameof(Items));
        }

        #endregion
    }
}
