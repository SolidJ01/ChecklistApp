using System.Windows.Input;
using ChecklistApp.Model;
using ChecklistApp.Services;

namespace ChecklistApp.ViewModel;

public class SelectableColorViewModel(string icon = "", string smallIcon = "", string selectedIcon = "", string selectedSmallIcon = "") : ViewModel
{
    private Checklist.ChecklistColor _color;
    private bool _selected;

    public Checklist.ChecklistColor Color
    {
        get { return _color; }
        set
        {
            _color = value;
            OnPropertyChanged(nameof(Color));
            OnPropertyChanged(nameof(SmallIcon));
        }
    }
    public int? CustomColorId { get; set; }

    public bool Selected
    {
        get { return _selected; }
        set
        {
            if (_selected == value)
                return;
            _selected = value;
            OnPropertyChanged(nameof(Selected));
            OnPropertyChanged(nameof(Icon));
            OnPropertyChanged(nameof(SmallIcon));
        }
    }
    public ICommand Command { get; set; }
    public ICommand SelectedCommand { get; set; }
    
    public string Icon
    {
        get
        {
            return Selected ? selectedIcon : icon;
        }
    }

    public string SmallIcon
    {
        get
        {
            return Color == Checklist.ChecklistColor.Custom ? Selected ? selectedSmallIcon : smallIcon : "";
        }
    }
}