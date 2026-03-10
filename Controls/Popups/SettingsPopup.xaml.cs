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
    public static readonly BindableProperty ViewModelProperty = BindableProperty.Create(nameof(ViewModel), typeof(SettingsPopupViewModel), typeof(SettingsPopup));

    public SettingsPopupViewModel ViewModel
    {
        get => (SettingsPopupViewModel)GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }
    
    public event EventHandler ExportChecklistsClicked;
    
    public SettingsPopup()
    {
        InitializeComponent();
    }

    protected override void CloseButtonClicked(object sender, EventArgs e)
    {
        Close();
    }

    private void ExportButtonClicked(object sender, EventArgs e)
    {
        ExportChecklistsClicked?.Invoke(sender, e);
    }
}