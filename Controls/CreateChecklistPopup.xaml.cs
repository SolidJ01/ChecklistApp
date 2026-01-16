using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ChecklistApp.Model;
using ChecklistApp.ViewModel;

namespace ChecklistApp.Controls;

public partial class CreateChecklistPopup : Popup
{
    public static readonly BindableProperty NameProperty = BindableProperty.Create(nameof(Name), typeof(string), typeof(CreateChecklistPopup), string.Empty,  BindingMode.TwoWay);
    public static readonly BindableProperty UseDeadlineProperty = BindableProperty.Create(nameof(UseDeadline), typeof(bool),  typeof(CreateChecklistPopup), false, BindingMode.TwoWay);
    public static readonly BindableProperty DeadlineProperty = BindableProperty.Create(nameof(Deadline), typeof(DateTime), typeof(CreateChecklistPopup), DateTime.Now, BindingMode.TwoWay);
    public static readonly BindableProperty ChecklistColorProperty = BindableProperty.Create(nameof(ChecklistColor), typeof(Checklist.ChecklistColor),  typeof(CreateChecklistPopup), Checklist.ChecklistColor.Grey, BindingMode.TwoWay);
    public static readonly BindableProperty SaveCommandProperty = BindableProperty.Create(nameof(SaveCommand), typeof(ICommand), typeof(CreateChecklistPopup));
    public static readonly BindableProperty CancelCommandProperty = BindableProperty.Create(nameof(CancelCommand), typeof(ICommand), typeof(CreateChecklistPopup));
    public static readonly BindableProperty ImportCommandProperty = BindableProperty.Create(nameof(ImportCommand), typeof(ICommand), typeof(CreateChecklistPopup));

    public string Name
    {
        get => (string)GetValue(NameProperty);
        set => SetValue(NameProperty, value);
    }

    public bool UseDeadline
    {
        get => (bool)GetValue(UseDeadlineProperty);
        set => SetValue(UseDeadlineProperty, value);
    }

    public DateTime Deadline
    {
        get => (DateTime)GetValue(DeadlineProperty);
        set => SetValue(DeadlineProperty, value);
    }

    public Checklist.ChecklistColor ChecklistColor
    {
        get => (Checklist.ChecklistColor)GetValue(ChecklistColorProperty);
        set => SetValue(ChecklistColorProperty, value);
    }
    
    public ICommand SaveCommand
    {
        get => (ICommand)GetValue(SaveCommandProperty);
        set => SetValue(SaveCommandProperty, value);
    }

    public ICommand CancelCommand
    {
        get => (ICommand)GetValue(CancelCommandProperty);
        set => SetValue(CancelCommandProperty, value);
    }
    
    public ICommand ImportCommand
    {
        get => (ICommand)GetValue(ImportCommandProperty);
        set => SetValue(ImportCommandProperty, value);
    }
    
    public CreateChecklistPopup()
    {
        InitializeComponent();
    }

    private void CloseButtonClicked(object sender, EventArgs e)
    {
        CancelCommand.Execute(null);
        Close();
    }

    private void ImportButtonClicked(object sender, EventArgs e)
    {
        ImportCommand.Execute(new Action(Close));
    }

    private void SaveButtonClicked(object sender, EventArgs e)
    {
        SaveCommand.Execute(new Action(Close));
    }
}