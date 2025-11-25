using ChecklistApp.Data;
using ChecklistApp.Model;
using ChecklistApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Maui.Storage;

namespace ChecklistApp.ViewModel
{
    [QueryProperty(nameof(Id), "id")]
    public class ChecklistOptionsPageViewModel : ViewModel
    {
        private ChecklistContext _checklistContext;
        private NavigationService _navigationService;
        private IFileSaver _fileSaver;

        private Checklist _checklist;
        private Checklist _unchangedChecklist;

        #region Properties

        public int Id { get; set; }
        public string Name { get { return _checklist != null ? _checklist.Name : "Loading"; } set { if (_checklist != null) _checklist.Name = value; } }
        public bool UseDeadline { get { return _checklist != null ? _checklist.UseDeadline : false; } set { if (_checklist != null) _checklist.UseDeadline = value; } }
        public DateTime Deadline { get { return _checklist != null ? _checklist.Deadline : DateTime.Now; } set { if (_checklist != null) _checklist.Deadline = value; } }
        public Checklist.ChecklistColor Color { get { return _checklist != null ? _checklist.Color : Checklist.ChecklistColor.Grey; } set { if (_checklist != null) _checklist.Color = value; } }

        #endregion

        #region Commands

        public ICommand BackCommand { get; set; }

        public ICommand SaveCommand { get; set; }

        public ICommand DeleteCommand { get; set; }
        
        public ICommand ExportCommand { get; set; }

        #endregion

        public ChecklistOptionsPageViewModel(ChecklistContext checklistContext, NavigationService navigationService, IFileSaver fileSaver)
        {
            _checklistContext = checklistContext;
            _navigationService = navigationService;
            _fileSaver = fileSaver;

            BackCommand = new Command(Back);
            SaveCommand = new Command(Save);
            DeleteCommand = new Command(Delete);
            ExportCommand = new Command(Export);
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

        private void Export()
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

        public async void RetrieveChecklist(object sender, EventArgs e)
        {
            _checklist = await _checklistContext.GetChecklist(Id);
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(UseDeadline));
            OnPropertyChanged(nameof(Deadline));
            OnPropertyChanged(nameof(Color));
        }

        #endregion
    }
}
