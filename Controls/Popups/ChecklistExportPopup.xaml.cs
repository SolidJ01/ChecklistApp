using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ChecklistApp.Model;
using ChecklistApp.ViewModel;

namespace ChecklistApp.Controls;

public partial class ChecklistExportPopup : Popup
{
    public static readonly BindableProperty ChecklistsProperty = BindableProperty.Create(nameof(Checklists), typeof(ObservableCollection<SelectableChecklistCardViewModel>), typeof(ChecklistExportPopup), propertyChanged:ChecklistsPropertyChanged);

    private static void ChecklistsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        ObservableCollection<SelectableChecklistCardViewModel> oldChecklists = oldValue as ObservableCollection<SelectableChecklistCardViewModel>;
        ObservableCollection<SelectableChecklistCardViewModel> newChecklists = newValue as ObservableCollection<SelectableChecklistCardViewModel>;
        ChecklistExportPopup bindablePopup = bindable as ChecklistExportPopup;
        if (newChecklists is not null)
        {
            foreach (var checklist in newChecklists)
            {
                checklist.PropertyChanged += bindablePopup.OnChecklistPropertyChanged;
            }
        }
        
    }

    private void OnChecklistPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        
    }

    public static readonly BindableProperty ExportCommandProperty = BindableProperty.Create(nameof(ExportCommand), typeof(ICommand), typeof(ChecklistExportPopup));

    public ObservableCollection<SelectableChecklistCardViewModel> Checklists
    {
        get => (ObservableCollection<SelectableChecklistCardViewModel>)GetValue(ChecklistsProperty);
        set => SetValue(ChecklistsProperty, value);
    }

    public ICommand ExportCommand
    {
        get => (ICommand)GetValue(ExportCommandProperty);
        set => SetValue(ExportCommandProperty, value);
    }

    public bool AllSelected
    {
        get { return Checklists.All(x => x.Selected); }
    }
    
    public ICommand ToggleAllCommand { get; set; }
    
    public ICommand UpdateAllSelectedCommand { get; set; }
    
    public ChecklistExportPopup()
    {
        ToggleAllCommand = new Command(ToggleAll);
        UpdateAllSelectedCommand = new Command(UpdateAllSelected);
        InitializeComponent();
    }

    private void CloseButtonClicked(object sender, EventArgs e)
    {
        Close();
    }

    private void ToggleAll()
    {
        bool targetCondition = !AllSelected;
        foreach (var checklist in Checklists)
        {
            checklist.Selected = targetCondition;
        }
        OnPropertyChanged(nameof(AllSelected));
    }

    private void UpdateAllSelected()
    {
        OnPropertyChanged(nameof(AllSelected));
    }

    private void ExportButtonClicked(object sender, EventArgs e)
    {
        ExportCommand?.Execute(new Action<Action>(Close));
    }
}