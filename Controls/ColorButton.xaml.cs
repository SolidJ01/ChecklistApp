namespace ChecklistApp.Controls;

public partial class ColorButton : ContentView
{
	public static readonly new BindableProperty BackgroundColorProperty = BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(ColorButton));

	public new Color BackgroundColor
	{
		get => (Color)GetValue(BackgroundColorProperty);
		set => SetValue(BackgroundColorProperty, value);
	}

	public ColorButton()
	{
		InitializeComponent();
	}
}