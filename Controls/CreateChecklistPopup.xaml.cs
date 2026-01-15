using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChecklistApp.Model;
using ChecklistApp.ViewModel;

namespace ChecklistApp.Controls;

public partial class CreateChecklistPopup : Popup
{
    private Checklist _checklist = new();

    public string Name
    {
        get => _checklist.Name;
        set
        {
            _checklist.Name = value;
            OnPropertyChanged();
        }
    }

    public bool UseDeadline
    {
        get => _checklist.UseDeadline;
        set
        {
            _checklist.UseDeadline = value;
            OnPropertyChanged();
        }
    }

    public DateTime Deadline
    {
        get => _checklist.Deadline;
        set
        {
            _checklist.Deadline = value;
            OnPropertyChanged();
        }
    }

    public Checklist.ChecklistColor Color
    {
        get => _checklist.Color;
        set
        {
            _checklist.Color = value;
            OnPropertyChanged();
        }
    }
    
    public CreateChecklistPopup()
    {
        InitializeComponent();
    }

    private void CloseButtonClicked(object sender, EventArgs e)
    {
        Close();
    }

    private void ImportButtonClicked(object sender, EventArgs e)
    {
        
    }

    private void SaveButtonClicked(object sender, EventArgs e)
    {
        
    }
}