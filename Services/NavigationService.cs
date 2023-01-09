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

                    break;
                case NavigationTarget.CreateItem:

                    break;
                case NavigationTarget.Back:
                    await Shell.Current.GoToAsync("..", true);
                    break;
            }
        }
    }
}
