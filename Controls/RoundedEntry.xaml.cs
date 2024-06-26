namespace ChecklistApp.Controls;

public partial class RoundedEntry : ContentView
{
	public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(RoundedEntry), string.Empty, BindingMode.TwoWay);
	public string Text
	{
		get => (string)GetValue(TextProperty);
		set => SetValue(TextProperty, value);
	}
	public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(RoundedEntry), string.Empty);
	public string Placeholder
	{
		get => (string)GetValue(PlaceholderProperty);
		set => SetValue(PlaceholderProperty, value);
	}
	public RoundedEntry()
	{
		InitializeComponent();
	}
}