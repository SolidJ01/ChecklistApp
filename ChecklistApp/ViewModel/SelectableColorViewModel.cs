using System.Windows.Input;
using ChecklistApp.Model;
using ChecklistApp.Services;

namespace ChecklistApp.ViewModel;

public class SelectableColorViewModel : ViewModel
{
    private bool _selected;
    public Checklist.ChecklistColor Color { get; set; }
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
        }
    }
    public ICommand Command { get; set; }
    public ICommand SelectedCommand { get; set; }
}