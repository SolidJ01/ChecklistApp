using ChecklistApp.Model;
using ChecklistApp.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChecklistApp.Services
{
    public class NavigationService
    {
        public enum NavigationTarget
        {
            Home, 
            CreateChecklist, 
            Checklist, 
            ChecklistSettings,
            CreateItem, 
            Back
        }
        public async void NavigateTo(NavigationTarget target)
        {
            switch (target)
            {
                case NavigationTarget.Home:
                    await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
                    break;
                case NavigationTarget.CreateChecklist:
                    await Shell.Current.GoToAsync($"{nameof(CreateChecklistPage)}");
                    break;
                case NavigationTarget.Checklist:
                    throw new Exception("Must pass a Checklist index to navigate to");
                case NavigationTarget.ChecklistSettings:
                    throw new Exception("Must pass a Checklist index to navigate to");
                case NavigationTarget.CreateItem:

                    break;
                case NavigationTarget.Back:
                    await Shell.Current.GoToAsync("..", true);
                    break;
            }
        }
        public async void NavigateTo(NavigationTarget target, int id)
        {
            switch (target)
            {
                case NavigationTarget.Checklist:
                    await Shell.Current.GoToAsync($"{nameof(ChecklistPage)}?id={id}");
                    break;
                case NavigationTarget.ChecklistSettings:

                    break;
                default:
                    NavigateTo(target);
                    break;
            }
        }
    }
}
