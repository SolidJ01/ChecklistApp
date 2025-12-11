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

public partial class SettingsPopup : Popup
{
    public static readonly BindableProperty ChecklistsProperty = BindableProperty.Create(nameof(Checklists), typeof(ObservableCollection<Checklist>), typeof(SettingsPopup));
    public static readonly BindableProperty ExportChecklistsCommandProperty = BindableProperty.Create(nameof(ExportChecklists), typeof(ICommand), typeof(SettingsPopup));

    public ObservableCollection<ChecklistCardViewModel> Checklists
    {
        get => (ObservableCollection<ChecklistCardViewModel>)GetValue(ChecklistsProperty);
        set => SetValue(ChecklistsProperty, value);
    }
    public ICommand ExportChecklists
    {
        get => (ICommand)GetValue(ExportChecklistsCommandProperty);
        set => SetValue(ExportChecklistsCommandProperty, value);
    }
    
    public SettingsPopup()
    {
        InitializeComponent();
    }

    private void CloseButtonClicked(object sender, EventArgs e)
    {
        Close();
    }

    private void ExportChecklistsButtonClicked(object sender, EventArgs e)
    {
        ChecklistExportPopup.Open();
    }
}