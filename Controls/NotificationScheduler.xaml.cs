using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ChecklistApp.Model;
using ChecklistApp.ViewModel;

namespace ChecklistApp.Controls;

public partial class NotificationScheduler : ContentView
{
    public static readonly BindableProperty NotificationsEnabledProperty = BindableProperty.Create(nameof(NotificationsEnabled), typeof(bool), typeof(NotificationScheduler), false);
    public static readonly BindableProperty NotificationsProperty = BindableProperty.Create(nameof(Notifications), typeof(ObservableCollection<NotificationViewModel>), typeof(NotificationScheduler), new ObservableCollection<NotificationViewModel>(), BindingMode.TwoWay);

    private bool _useNotifications = false;
    
    public bool NotificationsEnabled
    {
        get => (bool)GetValue(NotificationsEnabledProperty);
        set => SetValue(NotificationsEnabledProperty, value);
    }
    public ObservableCollection<NotificationViewModel> Notifications
    {
        get => (ObservableCollection<NotificationViewModel>)GetValue(NotificationsProperty);
        set => SetValue(NotificationsProperty, value);
    }

    public bool UseNotifications
    {
        get => _useNotifications;
        set
        {
            _useNotifications = value;
            OnPropertyChanged();
        }
    }
    
    public ICommand AddCommand { get; set; }
    
    public NotificationScheduler()
    {
        AddCommand = new Command(Add);
        InitializeComponent();
    }

    private void Add()
    {
        TimeSpan timeSpan = new TimeSpan(0,30,0);
        if (Notifications.Any())
        {
            NotificationViewModel last = Notifications.Last();
            switch (last.Scale)
            {
                case NotificationViewModel.TimeScale.Minutes:
                    timeSpan = new TimeSpan(0, last.Value + 10, 0);
                    break;
                case NotificationViewModel.TimeScale.Hours:
                    timeSpan = new TimeSpan(last.Value + 1, 0, 0);
                    break;
                case NotificationViewModel.TimeScale.Days:
                    timeSpan = new TimeSpan(last.Value + 1, 0, 0, 0);
                    break;
            }
        }
        Notifications.Add(new NotificationViewModel(new Notification { Value = timeSpan }));
    }
}