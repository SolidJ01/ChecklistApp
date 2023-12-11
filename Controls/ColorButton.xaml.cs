using System.Windows.Input;

namespace ChecklistApp.Controls;

public partial class ColorButton : ContentView
{
	public static readonly new BindableProperty BackgroundColorProperty = BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(ColorButton));

	public new Color BackgroundColor
	{
		get => (Color)GetValue(BackgroundColorProperty);
		set => SetValue(BackgroundColorProperty, value);
	}


	//	TODO: Research property change notification propagation - can this object be notified when the selected color changes through this property? 
	//public static readonly BindableProperty SelectedColorProperty = BindableProperty.Create(nameof(SelectedColor), typeof(Color), typeof(ColorButton));

	//public Color SelectedColor
	//{
	//	get => (Color)GetValue(SelectedColorProperty);
	//	set => SetValue(SelectedColorProperty, value);
	//}

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
		Command.Execute(CommandParameter);
    }
}