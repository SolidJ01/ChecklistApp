namespace ChecklistApp.Controls;

public partial class ChecklistProgressBar : ContentView
{
	public static BindableProperty ProgressProperty = BindableProperty.Create("Progress", typeof(float), typeof(ChecklistProgressBar), 0f, propertyChanged:ProgressChanged);

	private static void ProgressChanged(BindableObject bindable, object oldValue, object newValue)
	{
		ChecklistProgressBar checklistProgressBar = (ChecklistProgressBar)bindable;
		checklistProgressBar.OnProgressChanged();
	}

	public float Progress
	{
		get =>  (float)GetValue(ProgressProperty);
		set => SetValue(ProgressProperty, value);
	}

	private float priorProgress = 0;
	
	public ChecklistProgressBar()
	{
		priorProgress = Progress;
		InitializeComponent();
		ProgressContainer.SetLayoutBounds(ProgressIndicator, new Rect(0, 0, Progress, 1));
	}

	public void OnProgressChanged()
	{
		var animation = new Animation(v => ProgressContainer.SetLayoutBounds(ProgressIndicator, new Rect(0, 0, v, 1)),
			priorProgress, Progress);
		animation.Commit(this, "Progress", 16, 750, Easing.CubicInOut, (v, c) => priorProgress = Progress);
	}
}