using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System.Windows.Input;

namespace ChecklistApp.Controls;

public partial class RoundedCheckbox : ContentView
{
	public enum CheckboxStyle
	{
        DynamicForegroundDark, 
		StaticForeground, 
		StaticForegroundCyan, 
		StaticForegroundBlue, 
		StaticForegroundPurple, 
		StaticForegroundMagenta, 
		StaticForegroundRed, 
		StaticForegroundOrange, 
		StaticForegroundGreen
	}


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

	public static readonly BindableProperty TextColorCheckedProperty = BindableProperty.Create(nameof(TextColorChecked), typeof(Color), typeof(RoundedCheckbox), Colors.White);
	public Color TextColorChecked
	{
		get => (Color)GetValue(TextColorCheckedProperty);
		set => SetValue(TextColorCheckedProperty, value);
	}

	//public static new readonly BindableProperty BackgroundProperty = BindableProperty.Create(nameof(Background), typeof(Brush), typeof(RoundedCheckbox));
	//public new Brush Background
	//{
	//	get => (Brush)GetValue(BackgroundProperty);
	//	set => SetValue(BackgroundProperty, value);
	//}

	//public static readonly BindableProperty BackgroundCheckedProperty = BindableProperty.Create(nameof(BackgroundChecked), typeof(Brush), typeof(RoundedCheckbox));
	//public Brush BackgroundChecked
	//{
	//	get => (Brush)GetValue(BackgroundCheckedProperty);
	//	set => SetValue(BackgroundCheckedProperty, value);
	//}

	public static readonly BindableProperty DesignProperty = BindableProperty.Create(nameof(Design), typeof(CheckboxStyle), typeof(RoundedCheckbox), CheckboxStyle.DynamicForegroundDark);

    public CheckboxStyle Design
	{
		get => (CheckboxStyle)GetValue(DesignProperty);
		set => SetValue(DesignProperty, value);
	}

	public static readonly BindableProperty ClickablePaddingProperty = BindableProperty.Create(nameof(ClickablePadding), typeof(Thickness), typeof(RoundedCheckbox), Thickness.Zero);
	public Thickness ClickablePadding
	{
		get => (Thickness)GetValue(ClickablePaddingProperty);
		set => SetValue(ClickablePaddingProperty, value);
	}

	public static readonly BindableProperty EnsureSquareProperty = BindableProperty.Create(nameof(EnsureSquare), typeof(bool), typeof(RoundedCheckbox), false);
	public bool EnsureSquare
	{
		get => (bool)GetValue(EnsureSquareProperty);
		set => SetValue(EnsureSquareProperty, value);
	}

	public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(RoundedCheckbox), null);
	public ICommand Command
	{
		get => (ICommand)GetValue(CommandProperty);
		set => SetValue(CommandProperty, value);
	}

	public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(RoundedCheckbox), null);
	public object CommandParameter
	{
		get => (object)GetValue(CommandParameterProperty);
		set => SetValue(CommandParameterProperty, value);
	}

	public RoundedCheckbox()
	{
		InitializeComponent();
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
        IsChecked = !IsChecked;
        OnPropertyChanged(nameof(BaseFrame.Width));
        Command?.Execute(CommandParameter);
    }
}