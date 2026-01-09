using ChecklistApp.Model;

namespace ChecklistApp.ViewModel;

public class ItemViewModel : ViewModel
{
    private Item _item;

    public int Id
    {
        get { return _item.Id; }
    }

    public string Name
    {
        get { return _item.Name; }
        set 
        {
            _item.Name = value;
            OnPropertyChanged();
        }
    }

    public bool IsChecked
    {
        get { return _item.IsChecked; }
        set
        {
            _item.IsChecked = value;
            OnPropertyChanged();
        }
    }

    public ItemViewModel(Item item)
    {
        _item = item;
    }

    public void Update()
    {
        OnPropertyChanged(nameof(Name));
        OnPropertyChanged(nameof(IsChecked));
    }
}