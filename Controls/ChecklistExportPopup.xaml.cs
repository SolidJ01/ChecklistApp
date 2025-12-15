using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ChecklistApp.Model;
using ChecklistApp.ViewModel;

namespace ChecklistApp.Controls;

public partial class ChecklistExportPopup : Popup
{
    public static readonly BindableProperty ChecklistsProperty = BindableProperty.Create(nameof(Checklists), typeof(ObservableCollection<ChecklistCardViewModel>), typeof(ChecklistExportPopup));
    public static readonly BindableProperty ExportCommandProperty = BindableProperty.Create(nameof(ExportCommand), typeof(ICommand), typeof(ChecklistExportPopup));

    public ObservableCollection<ChecklistCardViewModel> Checklists
    {
        get => (ObservableCollection<ChecklistCardViewModel>)GetValue(ChecklistsProperty);
        set => SetValue(ChecklistsProperty, value);
    }

    public ICommand ExportCommand
    {
        get => (ICommand)GetValue(ExportCommandProperty);
        set => SetValue(ExportCommandProperty, value);
    }

    public ObservableCollection<Item> SelectableChecklists { get; private set; } = new ObservableCollection<Item>();
    
    public ChecklistExportPopup()
    {
        InitializeComponent();
    }

    private void CloseButtonClicked(object sender, EventArgs e)
    {
        Close();
    }
}