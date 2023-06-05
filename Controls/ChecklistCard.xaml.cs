using System.Windows.Input;

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

	public static readonly BindableProperty IsOverdueProperty = BindableProperty.Create(nameof(IsOverdue), typeof(bool), typeof(ChecklistCard), false);

    public bool IsOverdue
	{
		get => (bool)GetValue(IsOverdueProperty);
		set => SetValue(IsOverdueProperty, value);
	}

	public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ChecklistCard), null);
	public ICommand Command
	{
		get => (ICommand)GetValue(CommandProperty);
		set => SetValue(CommandProperty, value);
	}

	public static readonly BindableProperty ChecklistIdProperty = BindableProperty.Create(nameof(ChecklistId), typeof(int), typeof(ChecklistCard), null);
	public int ChecklistId
	{
		get => (int)GetValue(ChecklistIdProperty);
		set => SetValue(ChecklistIdProperty, value);
	}

    public ChecklistCard()
	{
		InitializeComponent();
	}
}