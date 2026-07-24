using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ChecklistApp.ViewModel;

namespace ChecklistApp.Controls;

public partial class UpdateChecker : ContentView
{
    public static readonly BindableProperty StateProperty = BindableProperty.Create(nameof(State), typeof(SettingsPopupViewModel.RefreshState), typeof(UpdateChecker), SettingsPopupViewModel.RefreshState.Idle);
    public static readonly BindableProperty RefreshCommandProperty = BindableProperty.Create(nameof(RefreshCommand), typeof(ICommand), typeof(UpdateChecker));
    public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(UpdateChecker));
    public static readonly BindableProperty SubtitleProperty = BindableProperty.Create(nameof(Subtitle), typeof(string), typeof(UpdateChecker));

    public SettingsPopupViewModel.RefreshState State
    {
        get => (SettingsPopupViewModel.RefreshState)GetValue(StateProperty);
        set => SetValue(StateProperty, value);
    }

    public ICommand RefreshCommand
    {
        get => (ICommand)GetValue(RefreshCommandProperty);
        set => SetValue(RefreshCommandProperty, value);
    }

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string Subtitle
    {
        get => (string)GetValue(SubtitleProperty);
        set => SetValue(SubtitleProperty, value);
    }
    
    public UpdateChecker()
    {
        InitializeComponent();
    }
}