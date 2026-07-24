using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChecklistApp.Model;

namespace ChecklistApp.Controls.Popups;

public partial class DialoguePopup : Popup
{
    public enum QueryType
    {
        Neutral,
        Negative,
        Positive
    }

    private string _query;
    private Action _confirmAction;
    private string _confirmPrompt;
    private Action _denyAction;
    private string _denyPrompt;
    private QueryType _type;

    public string Query
    {
        get => _query;
        set
        {
            _query = value;
            OnPropertyChanged();
        }
    }

    public Action ConfirmAction
    {
        get => _confirmAction;
        set
        {
            _confirmAction = value;
            OnPropertyChanged();
        }
    }

    public string ConfirmPrompt
    {
        get =>  _confirmPrompt;
        set
        {
            _confirmPrompt = value;
            OnPropertyChanged();
        }
        
    }

    public Action DenyAction
    {
        get => _denyAction;
        set
        {
            _denyAction = value;
            OnPropertyChanged();
        }
    }

    public string DenyPrompt
    {
        get => _denyPrompt;
        set
        {
            _denyPrompt = value;
            OnPropertyChanged();
        }
    }

    public QueryType Type
    {
        get => _type;
        set
        {
            _type = value;
            OnPropertyChanged();
        }
    }
    
    public DialoguePopup()
    {
        InitializeComponent();
    }

    public void Prompt(DialogueQueryEventArgs e)
    {
        Query = e.Query;
        ConfirmAction = e.ConfirmAction;
        ConfirmPrompt = e.ConfirmPrompt;
        DenyAction = e.DenyAction;
        DenyPrompt = e.DenyPrompt;
        Type = e.Type;
        Open();
    }

    protected override void Back()
    {
        Close(DenyAction);
    }

    private void DenyButtonClicked(object sender, EventArgs e)
    {
        Close(DenyAction);
    }

    private void ConfirmButtonClicked(object sender, EventArgs e)
    {
        ConfirmAction?.Invoke();
        Close();
    }
}