using System.Windows.Input;

namespace ChecklistApp.Controls;

public partial class RoundedCheckbox : ContentView
{
	public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(nameof(IsChecked), typeof(bool), typeof(RoundedCheckbox), false, BindingMode.TwoWay);
	public bool IsChecked
	{
		get => (bool)GetValue(IsCheckedProperty);
		set => SetValue(IsCheckedProperty, value);
	}
	public RoundedCheckbox()
	{
		InitializeComponent();
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
        IsChecked = !IsChecked;
    }
}