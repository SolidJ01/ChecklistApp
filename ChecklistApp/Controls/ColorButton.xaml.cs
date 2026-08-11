using System.Windows.Input;

namespace ChecklistApp.Controls;

public partial class ColorButton : ContentView
{
	public static readonly BindableProperty BackgroundBrushProperty = BindableProperty.Create(nameof(BackgroundBrush), typeof(Brush), typeof(ColorButton));

	public Brush BackgroundBrush
	{
		get => (Brush)GetValue(BackgroundBrushProperty);
		set => SetValue(BackgroundBrushProperty, value);
	}

	public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(nameof(IsChecked), typeof(bool), typeof(ColorButton), propertyChanged:OnIsCheckedPropertyChanged);

    public bool IsChecked
	{
		get => (bool)GetValue(IsCheckedProperty);
		set => SetValue(IsCheckedProperty, value);
    }

    private static void OnIsCheckedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var button = (ColorButton)bindable;
		button.OnPropertyChanged(nameof(IsChecked));
    }

	public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ColorButton));
	public ICommand Command
	{
		get => (ICommand)GetValue(CommandProperty);
		set => SetValue(CommandProperty, value);
	}

	public static readonly BindableProperty SelectedCommandProperty = BindableProperty.Create(nameof(SelectedCommand), typeof(ICommand), typeof(ColorButton));

	public ICommand SelectedCommand
	{
		get => (ICommand)GetValue(SelectedCommandProperty);
		set => SetValue(SelectedCommandProperty, value);
	}

    public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(ColorButton), null);
    public object CommandParameter
    {
        get => (object)GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
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