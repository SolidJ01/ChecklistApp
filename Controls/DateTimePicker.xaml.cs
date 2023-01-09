namespace ChecklistApp.Controls;

public partial class DateTimePicker : ContentView
{
	public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(DateTime), typeof(DateTimePicker), DateTime.Now, BindingMode.TwoWay);
	public DateTime Value
	{
		get => (DateTime)GetValue(ValueProperty);
		set => SetValue(ValueProperty, value);
	}
	public static readonly BindableProperty UseProperty = BindableProperty.Create(nameof(Use), typeof(bool), typeof(DateTimePicker), false, BindingMode.TwoWay);
	public bool Use
	{
		get => (bool)GetValue(UseProperty);
		set => SetValue(UseProperty, value);
	}
	public DateTimePicker()
	{
		InitializeComponent();
	}
}