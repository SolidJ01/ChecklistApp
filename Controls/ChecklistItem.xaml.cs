using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ChecklistApp.Model;

namespace ChecklistApp.Controls;

public partial class ChecklistItem : ContentView
{
    public static BindableProperty ItemProperty = BindableProperty.Create(nameof(Item), typeof(Item), typeof(ChecklistItem), propertyChanged: ItemPropertyChanged);
    public static BindableProperty ToggleCheckedCommandProperty = BindableProperty.Create(nameof(ToggleCheckedCommand), typeof(ICommand), typeof(ChecklistItem));
    public static BindableProperty DeleteItemCommandProperty = BindableProperty.Create(nameof(DeleteItemCommand), typeof(ICommand), typeof(ChecklistItem));
    public static BindableProperty SaveItemCommandProperty = BindableProperty.Create(nameof(SaveItemCommand), typeof(ICommand), typeof(ChecklistItem));
    
    private static void ItemPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var checklistItem = (ChecklistItem)bindable;
        checklistItem.OnItemPropertyChanged();
    }
    
    private bool _isEditing = false;
    private string _editedItemName = "";
    
    public Item Item
    {
        get => (Item)GetValue(ItemProperty);
        set => SetValue(ItemProperty, value);
    }

    public ICommand ToggleCheckedCommand
    {
        get => (ICommand)GetValue(ToggleCheckedCommandProperty);
        set => SetValue(ToggleCheckedCommandProperty, value);
    }

    public ICommand DeleteItemCommand
    {
        get => (ICommand)GetValue(DeleteItemCommandProperty);
        set => SetValue(DeleteItemCommandProperty, value);
    }

    public ICommand SaveItemCommand
    {
        get => (ICommand)GetValue(SaveItemCommandProperty);
        set => SetValue(SaveItemCommandProperty, value);
    }
    
    public bool IsEditing
    {
        get => _isEditing;
    }

    public string EditedItemName
    {
        get => _editedItemName;
        set
        {
            _editedItemName = value;
            OnPropertyChanged(nameof(EditedItemName));
        }
    }

    public ICommand ToggleIsEditingCommand { get; set; }
    public ICommand SaveChangesCommand { get; set; }
    
    public ChecklistItem()
    {
        ToggleIsEditingCommand = new Command(ToggleIsEditing);
        SaveChangesCommand = new Command(SaveChanges);
        InitializeComponent();
    }

    public void OnItemPropertyChanged()
    {
        EditedItemName = Item.Name;
    }

    private void ToggleIsEditing()
    {
        _isEditing = !_isEditing;
        OnPropertyChanged(nameof(IsEditing));
    }
    
    private void SaveChanges() 
    {
        if (_editedItemName != Item.Name)
        {
            Item.Name = _editedItemName;
            OnPropertyChanged(nameof(Item));
            SaveItemCommand?.Execute(Item);
        }
        ToggleIsEditing();
    }
}