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
    public static readonly BindableProperty Test_SendNotificationCommandProperty = BindableProperty.Create(nameof(Test_SendNotificationCommand), typeof(ICommand), typeof(SettingsPopup));
    public static readonly BindableProperty Test_CancelNotificationCommandProperty = BindableProperty.Create(nameof(Test_CancelNotificationCommand), typeof(ICommand), typeof(SettingsPopup));

    public ICommand Test_SendNotificationCommand
    {
        get => (ICommand)GetValue(Test_SendNotificationCommandProperty);
        set => SetValue(Test_SendNotificationCommandProperty, value);
    }

    public ICommand Test_CancelNotificationCommand
    {
        get => (ICommand)GetValue(Test_CancelNotificationCommandProperty);
        set => SetValue(Test_CancelNotificationCommandProperty, value);
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