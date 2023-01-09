namespace ChecklistApp.Controls;

public partial class ChecklistMetaEditor : ContentView
{
	public static readonly BindableProperty NameProperty = BindableProperty.Create(nameof(Name), typeof(string), typeof(ChecklistMetaEditor), string.Empty);
	public string Name
	{
		get => (string)GetValue(NameProperty);
		set => SetValue(NameProperty, value);
	}
	public static readonly BindableProperty UseDeadlineProperty = BindableProperty.Create(nameof(UseDeadline), typeof(bool), typeof(ChecklistMetaEditor), false);
	public bool UseDeadline
	{
		get => (bool)GetValue(UseDeadlineProperty);
		set => SetValue(UseDeadlineProperty, value);
	}
	public ChecklistMetaEditor()
	{
		InitializeComponent();
	}
}