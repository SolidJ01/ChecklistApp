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
using CommunityToolkit.Maui.Storage;

namespace ChecklistApp.ViewModel
{
    [QueryProperty(nameof(Id), "id")]
    public class ChecklistPageViewModel : ViewModel
    {
        private ChecklistContext _checklistContext;
        private NavigationService _navigationService;
        private IFileSaver _fileSaver;
        private Checklist _checklist;

        #region Properties

        public int Id { get; set; }

        //public Checklist Checklist { get; set; }

        public ChecklistCardViewModel ChecklistCardViewModel { get; set; }

        public ObservableCollection<ItemViewModel> Items { get; set; } = [];

        public Checklist ChecklistEditEntry { get; set; } = new() { Deadline = DateTime.Now };

        #endregion

        #region Commands

        public ICommand BackCommand { get; set; }

        public ICommand OptionsCommand { get; set; }

        public ICommand CreateNewCommand { get; set; }

        public ICommand ToggleItemCheckedCommand { get; set; }
        
        public ICommand SaveItemChangesCommand { get; set; }
        
        public ICommand DeleteItemCommand { get; set; }
        
        public ICommand ExportChecklistCommand { get; set; }
        
        public ICommand CancelChecklistEditCommand { get; set; }
        
        public ICommand SaveChecklistEditCommand { get; set; }
        
        public ICommand DeleteChecklistCommand { get; set; }

        #endregion

        public ChecklistPageViewModel(ChecklistContext checklistContext, NavigationService navigationService, IFileSaver fileSaver)
        {
            _checklistContext = checklistContext;
            _navigationService = navigationService;
            _fileSaver = fileSaver;

            BackCommand = new Command(Back);
            OptionsCommand = new Command(Options);
            CreateNewCommand = new Command(CreateNew);
            ToggleItemCheckedCommand = new Command<int>(ToggleItemChecked);
            SaveItemChangesCommand = new Command<(int, string)>(SaveItemChanges);
            DeleteItemCommand = new Command<int>(DeleteItem);
            
            ExportChecklistCommand = new Command(ExportChecklist);
            CancelChecklistEditCommand = new Command(CancelChecklistEdit);
            SaveChecklistEditCommand = new Command<Action>(SaveChecklist);
            DeleteChecklistCommand = new Command(DeleteChecklist);
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
            ChecklistEditEntry = new()
            {
                Name = _checklist.Name,
                UseDeadline = _checklist.UseDeadline,
                Deadline = _checklist.Deadline,
                Color = _checklist.Color
            };
            OnPropertyChanged(nameof(ChecklistEditEntry));
            
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

        private void ExportChecklist()
        {
            foreach (var item in _checklist.Items)
            {
                item.Checklist = null;
            }
            var stream = new MemoryStream(Encoding.Default.GetBytes(JsonSerializer.Serialize(_checklist)));
            _fileSaver.SaveAsync($"{_checklist.Name}.json", stream);
            foreach (var item in _checklist.Items)
            {
                item.Checklist = _checklist;
            }
        }

        private void CancelChecklistEdit()
        {
            ChecklistEditEntry = new()
            {
                Name = _checklist.Name,
                UseDeadline = _checklist.UseDeadline,
                Deadline = _checklist.Deadline,
                Color = _checklist.Color
            };
            OnPropertyChanged(nameof(ChecklistEditEntry));
        }

        private void SaveChecklist(Action callback = null)
        {
            _checklist.Name = ChecklistEditEntry.Name;
            _checklist.UseDeadline = ChecklistEditEntry.UseDeadline;
            _checklist.Deadline = ChecklistEditEntry.Deadline;
            _checklist.Color = ChecklistEditEntry.Color;
            _checklistContext.UpdateChecklist(_checklist);
            
            RetrieveChecklist(this, EventArgs.Empty);
            callback?.Invoke();
        }

        private void DeleteChecklist()
        {
            _checklistContext.DeleteChecklist(_checklist);
            _navigationService.NavigateTo(NavigationService.NavigationTarget.Home);
        }

        #endregion
    }
}
