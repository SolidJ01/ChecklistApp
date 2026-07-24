using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ChecklistApp.Model;

public class PropertyChangingItem : Item, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    private bool _isChecked;

    public override bool IsChecked
    {
        get => _isChecked;
        set
        {
            _isChecked = value;
            OnPropertyChanged(nameof(IsChecked));
        }
    }

    public PropertyChangingItem(Item item)
    {
        IsChecked = item.IsChecked;
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}