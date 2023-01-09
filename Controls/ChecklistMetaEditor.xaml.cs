namespace ChecklistApp.Controls;

public partial class ChecklistMetaEditor : ContentView
{
	public static readonly BindableProperty NameProperty = BindableProperty.Create(nameof(Name), typeof(string), typeof(ChecklistMetaEditor), string.Empty, BindingMode.TwoWay);
	public string Name
	{
		get => (string)GetValue(NameProperty);
		set => SetValue(NameProperty, value);
	}
	public static readonly BindableProperty UseDeadlineProperty = BindableProperty.Create(nameof(UseDeadline), typeof(bool), typeof(ChecklistMetaEditor), false, BindingMode.TwoWay);
	public bool UseDeadline
	{
		get => (bool)GetValue(UseDeadlineProperty);
		set => SetValue(UseDeadlineProperty, value);
	}
	public static readonly BindableProperty DeadlineProperty = BindableProperty.Create(nameof(Deadline), typeof(DateTime), typeof(ChecklistMetaEditor), DateTime.Now, BindingMode.TwoWay);
	public DateTime Deadline
	{
		get => (DateTime)GetValue(DeadlineProperty);
		set => SetValue(DeadlineProperty, value);
	}
	public ChecklistMetaEditor()
	{
		InitializeComponent();
	}
}