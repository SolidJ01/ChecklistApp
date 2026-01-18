using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ChecklistApp.Model;

namespace ChecklistApp.Controls;

public partial class ChecklistOptionsPopup : Popup
{
    public static readonly BindableProperty NameProperty = BindableProperty.Create(nameof(Name), typeof(string), typeof(ChecklistOptionsPopup), string.Empty, BindingMode.TwoWay);
    public static readonly BindableProperty UseDeadlineProperty = BindableProperty.Create(nameof(UseDeadline), typeof(bool), typeof(ChecklistOptionsPopup), false, BindingMode.TwoWay);
    public static readonly BindableProperty DeadlineProperty = BindableProperty.Create(nameof(Deadline), typeof(DateTime), typeof(ChecklistOptionsPopup), DateTime.Now, BindingMode.TwoWay);
    public static readonly BindableProperty ChecklistColorProperty = BindableProperty.Create(nameof(ChecklistColor), typeof(Checklist.ChecklistColor), typeof(ChecklistOptionsPopup), Checklist.ChecklistColor.Grey, BindingMode.TwoWay);
    public static readonly BindableProperty DeleteCommandProperty = BindableProperty.Create(nameof(DeleteCommand), typeof(ICommand), typeof(ChecklistOptionsPopup));
    public static readonly BindableProperty ExportCommandProperty = BindableProperty.Create(nameof(ExportCommand), typeof(ICommand), typeof(ChecklistOptionsPopup));
    public static readonly BindableProperty CancelCommandProperty = BindableProperty.Create(nameof(CancelCommand), typeof(ICommand), typeof(ChecklistOptionsPopup));
    public static readonly BindableProperty SaveCommandProperty = BindableProperty.Create(nameof(SaveCommand), typeof(ICommand), typeof(ChecklistOptionsPopup));

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

    public ICommand DeleteCommand
    {
        get => (ICommand)GetValue(DeleteCommandProperty);
        set => SetValue(DeleteCommandProperty, value);
    }

    public ICommand ExportCommand
    {
        get => (ICommand)GetValue(ExportCommandProperty);
        set => SetValue(ExportCommandProperty, value);
    }

    public ICommand CancelCommand
    {
        get => (ICommand)GetValue(CancelCommandProperty);
        set => SetValue(CancelCommandProperty, value);
    }

    public ICommand SaveCommand
    {
        get => (ICommand)GetValue(SaveCommandProperty);
        set => SetValue(SaveCommandProperty, value);
    }
    
    public ChecklistOptionsPopup()
    {
        InitializeComponent();
    }

    protected override void Back()
    {
        Close(() => CancelCommand.Execute(null));
    }

    protected override void CloseButtonClicked(object sender, EventArgs e)
    {
        Back();
    }

    private void SaveButtonClicked(object sender, EventArgs e)
    {
        SaveCommand.Execute(() =>
        {
            Close();
        });
    }
}