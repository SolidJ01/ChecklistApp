namespace ChecklistApp.Controls;

public partial class ChecklistCard : ContentView
{
    public static readonly BindableProperty NameProperty = BindableProperty.Create(nameof(Name), typeof(string), typeof(ChecklistCard), string.Empty);
	public string Name
	{
		get => (string)GetValue(NameProperty);
		set => SetValue(NameProperty, value);
	}

	public static readonly BindableProperty CompletionStatusProperty = BindableProperty.Create(nameof(CompletionStatus), typeof(string), typeof(ChecklistCard), string.Empty);
	public string CompletionStatus
	{
		get => (string)GetValue(CompletionStatusProperty);
		set => SetValue(CompletionStatusProperty, value);
	}

	public static readonly BindableProperty DeadlineStatusProperty = BindableProperty.Create(nameof(DeadlineStatus), typeof(string), typeof(ChecklistCard), string.Empty);
	public string DeadlineStatus
	{
		get => (string)GetValue(DeadlineStatusProperty);
		set => SetValue(DeadlineStatusProperty, value);
	}
    public ChecklistCard()
	{
		InitializeComponent();
	}
}