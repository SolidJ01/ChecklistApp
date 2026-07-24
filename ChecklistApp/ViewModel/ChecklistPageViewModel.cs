using ChecklistApp.Data;
using ChecklistApp.Model;
using ChecklistApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Storage;

namespace ChecklistApp.ViewModel
{
    [QueryProperty(nameof(Id), "id")]
    public class ChecklistPageViewModel : ViewModel
    {
        private ChecklistContext _checklistContext;
        private NavigationService _navigationService;
        private IFileSaver _fileSaver;
        private INotificationManagerService _notificationService;

        #region Properties

        public int Id { get; set; }

        public ChecklistViewModel Checklist { get; set; }
        
        public ChecklistOptionsPopupViewModel ChecklistOptions { get; }

        public ObservableCollection<Item> ItemsToAdd { get; set; } = [];

        #endregion

        #region Commands

        public ICommand BackCommand { get; set; }
        public ICommand ToggleItemCheckedCommand { get; set; }
        public ICommand SaveItemChangesCommand { get; set; }
        public ICommand DeleteItemCommand { get; set; }
        
        public ICommand AddNewItemCommand { get; set; }
        public ICommand DeleteNewItemCommand { get; set; }
        public ICommand SaveNewItemsCommand { get; set; }
        public ICommand CancelNewItemsCommand { get; set; }

        #endregion

        public ChecklistPageViewModel(ChecklistContext checklistContext, NavigationService navigationService, IFileSaver fileSaver, INotificationManagerService notificationService, ChecklistOptionsPopupViewModel checklistOptionsPopupViewModel)
        {
            _checklistContext = checklistContext;
            _navigationService = navigationService;
            _fileSaver = fileSaver;
            _notificationService = notificationService;
            ChecklistOptions = checklistOptionsPopupViewModel;
            ChecklistOptions.ChangesSaved += RetrieveChecklist;

            BackCommand = new Command(Back);
            ToggleItemCheckedCommand = new Command<int>(ToggleItemChecked);
            SaveItemChangesCommand = new Command<(int, string)>(SaveItemChanges);
            DeleteItemCommand = new Command<int>(DeleteItem);

            AddNewItemCommand = new Command(AddNewItem);
            DeleteNewItemCommand = new Command<Item>(DeleteNewItem);
            SaveNewItemsCommand = new Command<Action>(SaveNewItems);
            CancelNewItemsCommand = new Command(ResetItemsToAdd);
        }

        #region Methods

        private void Back()
        {
            _navigationService.NavigateTo(NavigationService.NavigationTarget.Back);
        }

        private void ToggleItemChecked(int id)
        {
            /*Item item = _checklist.Items.FirstOrDefault(x => x.Id.Equals(id));
            ItemViewModel viewModel = Items.FirstOrDefault(x => x.Id.Equals(id));
            if (item is null || viewModel is null)
                return;
            
            //viewModel.IsChecked = !viewModel.IsChecked;
            _checklist.Items = _checklist.Items.OrderBy(x => x.IsChecked).ThenBy(x => x.Name).ToList();
            Items.Remove(viewModel);
            Items.Insert(_checklist.Items.IndexOf(item), new ItemViewModel(item));

            ChecklistViewModel.Update(_checklist);
            
            _checklistContext.UpdateItem(item);*/
            Item item = Checklist.Items.First(x => x.Id == id).Item;
            _checklistContext.UpdateItem(item);
        }

        private void SaveItemChanges((int, string) data)
        {
            ItemViewModel item = Checklist.Items.FirstOrDefault(x => x.Id.Equals(data.Item1));
            item.Name = data.Item2;
            
            int newIndex = Checklist.Items.OrderBy(x => x.IsChecked).ThenBy(x => x.Name).ToList().IndexOf(item);
            Checklist.Items.Move(Checklist.Items.IndexOf(item), newIndex);
            
            _checklistContext.UpdateItem(item.Item);
        }

        private void DeleteItem(int id)
        {
            var checklistItem = Checklist.Items.FirstOrDefault(x => x.Id.Equals(id));
            if (checklistItem is null)
                return;

            Checklist.Checklist.Items.Remove(checklistItem.Item);
            Checklist.Update(Checklist.Checklist);
            
            _checklistContext.DeleteItem(checklistItem.Item);
        }

        public async void RetrieveChecklist(object sender, EventArgs e)
        {
            Checklist checklist = await _checklistContext.GetChecklist(Id);
            foreach (var item in checklist.Items)
                item.Checklist = checklist;
            checklist.Items = checklist.Items.OrderBy(x => x.IsChecked).ThenBy(x => x.Name).ToList();
            
            if (Checklist is null)
            {
                Checklist = new ChecklistViewModel(checklist);
                OnPropertyChanged(nameof(Checklist));
            }
            else
            {
                await Checklist.Update(checklist);
            }
            
            ChecklistOptions.Load(Id);
            ResetItemsToAdd();
        }
        
        #endregion
        
        #region CreateItemsMethods

        private void AddNewItem()
        {
            ItemsToAdd.Add(new Item { Checklist = Checklist.Checklist });
        }

        private void DeleteNewItem(Item item)
        {
            ItemsToAdd.Remove(item);
        }

        private void SaveNewItems(Action callback = null)
        {
            _checklistContext.CreateItems(ItemsToAdd.ToList());
            RetrieveChecklist(this, EventArgs.Empty);
            callback?.Invoke();
        }
        
        private void ResetItemsToAdd()
        {
            ItemsToAdd.Clear();
            AddNewItem();
        }

        #endregion
    }
}
