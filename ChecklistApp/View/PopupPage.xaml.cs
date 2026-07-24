using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChecklistApp.Controls;
using ChecklistApp.Model;

namespace ChecklistApp.View;

public partial class PopupPage : ContentPage
{
    private List<Action> _backButtonActions = [];

    public PopupPage()
    {
        InitializeComponent();
    }

    protected void OnBackButtonActionChanged(object sender, BackButtonActionRegisterEventArgs e)
    {
        switch (e.EventIntent)
        {
            case BackButtonActionRegisterEventArgs.Intent.Register:
                _backButtonActions.Add(e.Action);
                break;
            case BackButtonActionRegisterEventArgs.Intent.Deregister:
                if (!_backButtonActions.Contains(e.Action))
                    break;
                
                _backButtonActions.Remove(e.Action);
                break;
        }
    }

    protected override bool OnBackButtonPressed()
    {
        if (_backButtonActions.Count.Equals(0))
            return false;
		
        _backButtonActions.Last().Invoke();
        //_backButtonActions.RemoveAt(_backButtonActions.Count - 1);
        return true;
    }
}