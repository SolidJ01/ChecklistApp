using ChecklistApp.Model;

namespace ChecklistApp.ViewModel;

public class SelectableChecklistViewModel : ChecklistViewModel
{
    private bool _selected;

    public bool Selected
    {
        get { return _selected; }
        set
        {
            _selected = value;
            OnPropertyChanged();
        }
    }
    
    public SelectableChecklistViewModel(Checklist checklist, bool selected = false) : base(checklist)
    {
        Selected = selected;
    }
}