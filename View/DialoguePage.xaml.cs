using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChecklistApp.Controls.Popups;
using ChecklistApp.Model;

namespace ChecklistApp.View;

public partial class DialoguePage : PopupPage
{
    public DialoguePage()
    {
        InitializeComponent();
    }

    protected void OnQueryDialogue(object sender, DialogueQueryEventArgs e)
    {
        ((DialoguePopup)GetTemplateChild("Dialogue")).Prompt(e);
    }
}