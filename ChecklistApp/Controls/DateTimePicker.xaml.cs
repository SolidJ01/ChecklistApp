namespace ChecklistApp.Controls;

public partial class DateTimePicker : ContentView
{
	public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(DateTime), typeof(DateTimePicker), DateTime.Now, BindingMode.TwoWay, propertyChanged: OnValueChanged);
	public DateTime Value
	{
		get => (DateTime)GetValue(ValueProperty);
		set => SetValue(ValueProperty, value);
	}

	public DateTime Date
	{
		get
		{
			return Value;
		}
		set
		{
			Value = new DateTime(value.Year, value.Month, value.Day, Value.Hour, Value.Minute, Value.Second);
		}
	}

	public TimeSpan Time
	{
		get
		{
			return Value.TimeOfDay;
		}
		set
		{
			Value = new DateTime(Value.Year, Value.Month, Value.Day, value.Hours, value.Minutes, value.Seconds);
		}
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

	static void OnValueChanged(BindableObject bindable, object oldValue, object newValue)
	{
		System.Diagnostics.Debug.WriteLine($"Value on bindableobject {bindable.ToString} of type {bindable.GetType} changed");
		DateTimePicker dtp = (DateTimePicker)bindable;
		dtp.ValueChanged();
	}

	public void ValueChanged()
	{
		OnPropertyChanged(nameof(Date));
		OnPropertyChanged(nameof(Time));
	}
}