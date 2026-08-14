using System.Windows.Input;

namespace ChecklistApp.Controls;

public partial class ColorButton : ContentView
{
	public static readonly BindableProperty BackgroundBrushProperty = BindableProperty.Create(nameof(BackgroundBrush), typeof(Brush), typeof(ColorButton));
	public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(nameof(IsChecked), typeof(bool), typeof(ColorButton), propertyChanged:OnIsCheckedPropertyChanged);
	public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ColorButton));
	public static readonly BindableProperty SelectedCommandProperty = BindableProperty.Create(nameof(SelectedCommand), typeof(ICommand), typeof(ColorButton));
	public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(ColorButton), null);
	public static readonly BindableProperty IconFontSizeProperty = BindableProperty.Create(nameof(IconFontSize), typeof(double), typeof(ColorButton), 26.0);
	public static readonly BindableProperty SmallIconFontSizeProperty = BindableProperty.Create(nameof(SmallIconFontSize), typeof(double), typeof(ColorButton), 13.0);
	public static readonly BindableProperty IconProperty = BindableProperty.Create(nameof(Icon), typeof(string), typeof(ColorButton), "");
	public static readonly BindableProperty SmallIconProperty = BindableProperty.Create(nameof(SmallIcon), typeof(string), typeof(ColorButton), "");

	private static void OnIsCheckedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		var button = (ColorButton)bindable;
		button.OnPropertyChanged(nameof(IsChecked));
	}

	public Brush BackgroundBrush
	{
		get => (Brush)GetValue(BackgroundBrushProperty);
		set => SetValue(BackgroundBrushProperty, value);
	}

    public bool IsChecked
	{
		get => (bool)GetValue(IsCheckedProperty);
		set => SetValue(IsCheckedProperty, value);
    }

	public ICommand Command
	{
		get => (ICommand)GetValue(CommandProperty);
		set => SetValue(CommandProperty, value);
	}

	public ICommand SelectedCommand
	{
		get => (ICommand)GetValue(SelectedCommandProperty);
		set => SetValue(SelectedCommandProperty, value);
	}

    public object CommandParameter
    {
        get => (object)GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public double IconFontSize
    {
	    get => (double)GetValue(IconFontSizeProperty);
	    set => SetValue(IconFontSizeProperty, value);
    }

    public double SmallIconFontSize
    {
	    get => (double)GetValue(SmallIconFontSizeProperty);
	    set => SetValue(SmallIconFontSizeProperty, value);
    }

    public string Icon
    {
	    get => (string)GetValue(IconProperty);
	    set => SetValue(IconProperty, value);
    }

    public string SmallIcon
    {
	    get => (string)GetValue(SmallIconProperty);
	    set => SetValue(SmallIconProperty, value);
    }

    public ColorButton()
	{
		InitializeComponent();
	}

    private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
	    if (!IsChecked)
			Command?.Execute(CommandParameter);
	    else
		    SelectedCommand?.Execute(CommandParameter);
    }
}