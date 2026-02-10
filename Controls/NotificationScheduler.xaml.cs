using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChecklistApp.Model;

namespace ChecklistApp.Controls;

public partial class NotificationScheduler : ContentView
{
    public static readonly BindableProperty NotificationsEnabledProperty = BindableProperty.Create(nameof(NotificationsEnabled), typeof(bool), typeof(NotificationScheduler), false);
    public static readonly BindableProperty NotificationsProperty = BindableProperty.Create(nameof(Notifications), typeof(ObservableCollection<Notification>), typeof(NotificationScheduler), new ObservableCollection<Notification>());

    public bool NotificationsEnabled
    {
        get => (bool)GetValue(NotificationsEnabledProperty);
        set => SetValue(NotificationsEnabledProperty, value);
    }
    public ObservableCollection<Notification> Notifications
    {
        get => (ObservableCollection<Notification>)GetValue(NotificationsProperty);
        set => SetValue(NotificationsProperty, value);
    }
    
    public bool Enabled { get; set; }
    
    public NotificationScheduler()
    {
        InitializeComponent();
    }
}