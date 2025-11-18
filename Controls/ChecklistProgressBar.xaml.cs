namespace ChecklistApp.Controls;

public partial class ChecklistProgressBar : ContentView
{
	public static BindableProperty ProgressProperty = BindableProperty.Create("Progress", typeof(float), typeof(ChecklistProgressBar), 0f);

	public float Progress
	{
		get =>  (float)GetValue(ProgressProperty);
		set => SetValue(ProgressProperty, value);
	}
	
	public ChecklistProgressBar()
	{
		InitializeComponent();
	}
}