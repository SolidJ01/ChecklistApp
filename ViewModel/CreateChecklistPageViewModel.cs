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

namespace ChecklistApp.ViewModel
{
    public class CreateChecklistPageViewModel : ViewModel
    {
        private ChecklistContext _checklistContext;
        private NavigationService _navigationService;
        private Checklist _checklist;

        #region Properties

        public string Name { get { return _checklist.Name; } set { _checklist.Name = value; } }
        public bool UseDeadline { get { return _checklist.UseDeadline; } set { _checklist.UseDeadline = value; } }
        public DateTime Deadline { get { return _checklist.Deadline; } set { _checklist.Deadline = value; } }
        public Checklist.ChecklistColor Color { get { return _checklist.Color; } set { _checklist.Color = value; } }

        #endregion

        #region Commands

        public ICommand BackCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand ImportCommand { get; set; }

        #endregion

        public CreateChecklistPageViewModel(ChecklistContext checklistContext, NavigationService navigationService)
        {
            _checklistContext = checklistContext;
            _navigationService = navigationService;
            _checklist = new Checklist();
            _checklist.Deadline = DateTime.Now;

            BackCommand = new Command(Back);
            SaveCommand = new Command(Save);
            ImportCommand = new Command(Import);
        }

        private void Back()
        {
            _navigationService.NavigateTo(NavigationService.NavigationTarget.Back);
        }

        private void Save()
        {
            try
            {
                _checklist.Name = StringHelper.FormatItemName(Name);
                _checklistContext.CreateChecklist(_checklist);
                _navigationService.NavigateTo(NavigationService.NavigationTarget.Home);
            }
            catch
            {
                //  TODO: Notify user something went wrong
            }
        }

        private async void Import()
        {
            try
            {
                PickOptions options = new PickOptions
                {
                    PickerTitle = "Select a .json file",
                    FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                    {
                        { DevicePlatform.Android, ["application/json"] },
                    })
                };
                var result = await FilePicker.Default.PickAsync(options);
                if (result != null)
                {
                    var stream = await result.OpenReadAsync();
                    try
                    {
                        Checklist checklist = JsonSerializer.Deserialize<Checklist>(stream);
                        checklist.Id = 0;
                        foreach (Item item in checklist.Items)
                        {
                            item.Id = 0;
                        }

                        _checklistContext.CreateChecklist(checklist);
                    }
                    catch (JsonException e)
                    {
                        stream = await result.OpenReadAsync();
                        
                        List<Checklist> checklists = JsonSerializer.Deserialize<List<Checklist>>(stream);
                        foreach (Checklist checklist in checklists)
                        {
                            checklist.Id = 0;
                            foreach (Item item in checklist.Items)
                            {
                                item.Id = 0;
                                item.Checklist = checklist;
                            }
                            _checklistContext.CreateChecklist(checklist);
                        }
                        //_checklistContext.CreateChecklists(checklists);
                    }
                    Back();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
