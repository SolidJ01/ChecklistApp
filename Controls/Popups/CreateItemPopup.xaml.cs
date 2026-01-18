using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ChecklistApp.Model;

namespace ChecklistApp.Controls;

public partial class CreateItemPopup : Popup
{
    public static readonly BindableProperty ItemsProperty = BindableProperty.Create(nameof(Items), typeof(ObservableCollection<Item>), typeof(CreateItemPopup), new ObservableCollection<Item>(), BindingMode.TwoWay);
    public static readonly BindableProperty AddCommandProperty = BindableProperty.Create(nameof(AddCommand), typeof(ICommand), typeof(CreateItemPopup));
    public static readonly BindableProperty DeleteCommandProperty = BindableProperty.Create(nameof(DeleteCommand), typeof(ICommand), typeof(CreateItemPopup));
    public static readonly BindableProperty SaveCommandProperty = BindableProperty.Create(nameof(SaveCommand), typeof(ICommand), typeof(CreateItemPopup));
    public static readonly BindableProperty CancelCommandProperty = BindableProperty.Create(nameof(CancelCommand), typeof(ICommand), typeof(CreateItemPopup));

    public ObservableCollection<Item> Items
    {
        get => (ObservableCollection<Item>)GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }

    public ICommand AddCommand
    {
        get => (ICommand)GetValue(AddCommandProperty);
        set => SetValue(AddCommandProperty, value);
    }

    public ICommand DeleteCommand
    {
        get => (ICommand)GetValue(DeleteCommandProperty);
        set => SetValue(DeleteCommandProperty, value);
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
    
    public CreateItemPopup()
    {
        InitializeComponent();
    }

    public override void Open(Action<Action> backButtonRegisterCallback, Action<Action> backButtonDeregisterCallback)
    {
        backButtonRegisterCallback?.Invoke(Cancel);
        base.Open(backButtonRegisterCallback,  backButtonDeregisterCallback);
    }

    private void Cancel()
    {
        Close(() => CancelCommand.Execute(null));
    }

    protected override void CloseButtonClicked(object sender, EventArgs e)
    {
        _backButtonDeregisterCallback?.Invoke(Cancel);
        Cancel();
    }

    private void SaveButtonClicked(object sender, EventArgs e)
    {
        SaveCommand.Execute(() =>
        {
            _backButtonDeregisterCallback?.Invoke(Cancel);
            Close(() => CancelCommand.Execute(null));
        });
    }
}