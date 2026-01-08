using ChecklistApp.Model;

namespace ChecklistApp.ViewModel;

public class SelectableChecklistCardViewModel : ChecklistCardViewModel
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
    
    public SelectableChecklistCardViewModel(Checklist checklist, bool selected = false) : base(checklist)
    {
        Selected = selected;
    }
}