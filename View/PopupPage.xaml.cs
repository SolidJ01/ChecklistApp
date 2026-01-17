using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChecklistApp.View;

public partial class PopupPage : ContentPage
{
    private List<Action> _backButtonActions = [];

    public PopupPage()
    {
        InitializeComponent();
    }

    protected void RegisterBackButtonAction(Action action)
    {
        _backButtonActions.Add(action);
    }

    protected void DeregisterBackButtonAction(Action action)
    {
        if (!_backButtonActions.Contains(action))
            return;
		
        _backButtonActions.Remove(action);
    }

    protected override bool OnBackButtonPressed()
    {
        if (_backButtonActions.Count.Equals(0))
            return false;
		
        _backButtonActions.Last().Invoke();
        _backButtonActions.RemoveAt(_backButtonActions.Count - 1);
        return true;
    }
}