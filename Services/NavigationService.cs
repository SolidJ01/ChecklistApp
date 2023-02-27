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
            ChecklistOptions,
            CreateItem, 
            Back
        }
        public async void NavigateTo(NavigationTarget target)
        {
            switch (target)
            {
                case NavigationTarget.Home:
                    await Shell.Current.GoToAsync($"//{nameof(MainPage)}", true);
                    break;
                case NavigationTarget.CreateChecklist:
                    await Shell.Current.GoToAsync($"{nameof(CreateChecklistPage)}", true);
                    break;
                case NavigationTarget.Checklist:
                    throw new Exception("Must pass a Checklist index to navigate to");
                case NavigationTarget.ChecklistOptions:
                    throw new Exception("Must pass a Checklist index to navigate to");
                case NavigationTarget.CreateItem:
                    throw new Exception("Must pass a Checklist index to navigate to");
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
                case NavigationTarget.ChecklistOptions:
                    await Shell.Current.GoToAsync($"{nameof(ChecklistOptionsPage)}?id={id}");
                    break;
                case NavigationTarget.CreateItem:
                    await Shell.Current.GoToAsync($"{nameof(CreateItemPage)}?id={id}");
                    break;
                default:
                    NavigateTo(target);
                    break;
            }
        }
    }
}
