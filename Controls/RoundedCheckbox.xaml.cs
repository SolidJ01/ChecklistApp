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

	public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(nameof(FontSize), typeof(int), typeof(RoundedCheckbox), 25);
	public int FontSize
	{
		get => (int)GetValue(FontSizeProperty);
		set => SetValue(FontSizeProperty, value);
	}

	public static readonly BindableProperty ClickablePaddingProperty = BindableProperty.Create(nameof(ClickablePadding), typeof(Thickness), typeof(RoundedCheckbox), Thickness.Zero);
	public Thickness ClickablePadding
	{
		get => (Thickness)GetValue(ClickablePaddingProperty);
		set => SetValue(ClickablePaddingProperty, value);
	}

	public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(RoundedCheckbox), null);
	public ICommand Command
	{
		get => (ICommand)GetValue(CommandProperty);
		set => SetValue(CommandProperty, value);
	}
	public RoundedCheckbox()
	{
		InitializeComponent();
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
        IsChecked = !IsChecked;
		if (Command is not null)
		{
			Command.Execute(this);
		}
    }
}