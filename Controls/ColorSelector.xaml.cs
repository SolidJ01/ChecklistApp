namespace ChecklistApp.Controls;
using ChecklistApp.Model;

public partial class ColorSelector : ContentView
{
	public static readonly BindableProperty ColorProperty = BindableProperty.Create(nameof(Color), typeof(Checklist.ChecklistColor), typeof(ColorSelector), Checklist.ChecklistColor.Grey, BindingMode.TwoWay);
	public Checklist.ChecklistColor Color
	{
		get => (Checklist.ChecklistColor)GetValue(ColorProperty);
		set => SetValue(ColorProperty, value);
	}

	public ColorSelector()
	{
		InitializeComponent();
	}
}