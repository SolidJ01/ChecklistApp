using System.Collections.ObjectModel;
using System.Windows.Input;
using ChecklistApp.Model;
using ChecklistApp.ViewModel;

namespace ChecklistApp.Controls;

public partial class ChecklistMetaEditor : ContentView
{
	public static readonly BindableProperty NameProperty = BindableProperty.Create(nameof(Name), typeof(string), typeof(ChecklistMetaEditor), string.Empty, BindingMode.TwoWay);
	public static readonly BindableProperty UseDeadlineProperty = BindableProperty.Create(nameof(UseDeadline), typeof(bool), typeof(ChecklistMetaEditor), false, BindingMode.TwoWay);
	public static readonly BindableProperty DeadlineProperty = BindableProperty.Create(nameof(Deadline), typeof(DateTime), typeof(ChecklistMetaEditor), DateTime.Now, BindingMode.TwoWay);
	public static readonly BindableProperty ChecklistColorProperty = BindableProperty.Create(nameof(ChecklistColor), typeof(Checklist.ChecklistColor), typeof(ChecklistMetaEditor), Checklist.ChecklistColor.Magenta, BindingMode.TwoWay, propertyChanged:OnChecklistColorChanged);
	public static readonly BindableProperty NotificationsEnabledProperty = BindableProperty.Create(nameof(NotificationsEnabled), typeof(bool), typeof(ChecklistMetaEditor), false, BindingMode.TwoWay);
	public static readonly BindableProperty NotificationsProperty = BindableProperty.Create(nameof(Notifications), typeof(ObservableCollection<NotificationViewModel>), typeof(ChecklistMetaEditor), new ObservableCollection<NotificationViewModel>(), BindingMode.TwoWay);

	private static void OnChecklistColorChanged(BindableObject bindable, object oldValue, object newValue)
	{
		var editor = (ChecklistMetaEditor)bindable;
		editor.OnPropertyChanged(nameof(ChecklistColor));
	}
	
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

	public Checklist.ChecklistColor ChecklistColor
	{
		get => (Checklist.ChecklistColor)GetValue(ChecklistColorProperty);
		set => SetValue(ChecklistColorProperty, value);
    }

	public ICommand UseDeadlineUpdatedComand { get; set; }

    public ChecklistMetaEditor()
	{
		UseDeadlineUpdatedComand = new Command(OnUseDeadlineUpdated);
		InitializeComponent();
	}

	private void OnUseDeadlineUpdated()
	{
		if (UseDeadline)
			return;

		NotificationScheduler.DisableNotifications();
		//NotificationsEnabled = false;
		//OnPropertyChanged(nameof(NotificationsEnabled));
	}
}