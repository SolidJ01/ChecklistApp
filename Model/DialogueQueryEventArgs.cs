using ChecklistApp.Controls.Popups;

namespace ChecklistApp.Model;

public class DialogueQueryEventArgs : EventArgs
{
    private string _query;
    private Action _confirmAction;
    private string _confirmPrompt;
    private Action _denyAction;
    private string _denyPrompt;
    private DialoguePopup.QueryType _type;

    public string Query => _query;
    public Action ConfirmAction =>  _confirmAction;
    public string ConfirmPrompt =>  _confirmPrompt;
    public Action DenyAction =>  _denyAction;
    public string DenyPrompt =>  _denyPrompt;
    public DialoguePopup.QueryType Type => _type;

    public DialogueQueryEventArgs(string query, Action confirmAction, string confirmPrompt, Action denyAction = null, string denyPrompt = "Cancel", DialoguePopup.QueryType type = DialoguePopup.QueryType.Neutral)
    {
        _query = query;
        _confirmAction = confirmAction;
        _confirmPrompt = confirmPrompt;
        _denyAction = denyAction;
        _denyPrompt = denyPrompt;
        _type = type;
    }
}