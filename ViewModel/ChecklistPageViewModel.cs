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

        //public Checklist Checklist { get; set; }

        public ChecklistCardViewModel ChecklistCardViewModel { get; set; }

        public ObservableCollection<ItemViewModel> Items { get; set; } = [];

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
            SaveItemChangesCommand = new Command<(int, string)>(SaveItemChanges);
            DeleteItemCommand = new Command<int>(DeleteItem);
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
            Item item = _checklist.Items.FirstOrDefault(x => x.Id.Equals(id));
            ItemViewModel viewModel = Items.FirstOrDefault(x => x.Id.Equals(id));
            if (item is null || viewModel is null)
                return;
            
            //viewModel.IsChecked = !viewModel.IsChecked;
            _checklist.Items = _checklist.Items.OrderBy(x => x.IsChecked).ThenBy(x => x.Name).ToList();
            Items.Remove(viewModel);
            Items.Insert(_checklist.Items.IndexOf(item), new ItemViewModel(item));

            ChecklistCardViewModel.Update(_checklist);
            
            _checklistContext.UpdateItem(item);
        }

        private void SaveItemChanges((int, string) data)
        {
            Item item = _checklist.Items.FirstOrDefault(x => x.Id.Equals(data.Item1));
            item.Name = data.Item2;
            
            _checklist.Items = _checklist.Items.OrderBy(x => x.IsChecked).ThenBy(x => x.Name).ToList();
            int newIndex = _checklist.Items.IndexOf(item);
            if (!newIndex.Equals(Items.IndexOf(Items.FirstOrDefault(x => x.Id.Equals(item.Id)))))
            {
                ItemViewModel viewModel = Items.FirstOrDefault(x => x.Id.Equals(item.Id));
                Items.Remove(viewModel);
                Items.Insert(newIndex, viewModel);
            }
            
            _checklistContext.UpdateItem(item);
        }

        private void DeleteItem(int id)
        {
            var checklistItem = _checklist.Items.FirstOrDefault(x => x.Id.Equals(id));
            _checklist.Items.Remove(checklistItem);
            Items.Remove(Items.FirstOrDefault(x => x.Id.Equals(id)));
            
            ChecklistCardViewModel.Update(_checklist);
            
            _checklistContext.DeleteItem(checklistItem);
        }

        public async void RetrieveChecklist(object sender, EventArgs e)
        {
            _checklist = await _checklistContext.GetChecklist(Id);
            if (ChecklistCardViewModel is null)
            {
                ChecklistCardViewModel = new ChecklistCardViewModel(_checklist);
                OnPropertyChanged(nameof(ChecklistCardViewModel));
            }
            else
            {
                ChecklistCardViewModel.Update(_checklist);
            }
            foreach (var item in _checklist.Items)
                item.Checklist = _checklist;
            
            _checklist.Items = _checklist.Items.OrderBy(x => x.IsChecked).ThenBy(x => x.Name).ToList();

            List<Item> addedItems = _checklist.Items.Where(x => !Items.Any(y => y.Id == x.Id)).ToList();
            List<ItemViewModel> removedItems = Items.Where(x => !_checklist.Items.Any(y => y.Id == x.Id)).ToList();

            await Task.Run(() =>
            {
                foreach (Item item in addedItems)
                    Items.Insert(_checklist.Items.IndexOf(item), new ItemViewModel(item));
                foreach (ItemViewModel item in removedItems)
                    Items.Remove(item);
            });
        }

        #endregion
    }
}
