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
    public event EventHandler ExportChecklistsClicked;
    
    public SettingsPopup()
    {
        InitializeComponent();
    }

    public override void Open(Action<Action> backButtonRegisterCallback, Action<Action> backButtonDeregisterCallback)
    {
        backButtonRegisterCallback?.Invoke(QuickClose);
        base.Open(backButtonRegisterCallback, backButtonDeregisterCallback);
    }

    protected override void CloseButtonClicked(object sender, EventArgs e)
    {
        _backButtonDeregisterCallback?.Invoke(QuickClose);
        Close();
    }

    private void ExportButtonClicked(object sender, EventArgs e)
    {
        ExportChecklistsClicked?.Invoke(sender, e);
    }
}