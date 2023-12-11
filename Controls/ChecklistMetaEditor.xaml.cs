using ChecklistApp.Model;

namespace ChecklistApp.Controls;

public partial class ChecklistMetaEditor : ContentView
{
	public static readonly BindableProperty NameProperty = BindableProperty.Create(nameof(Name), typeof(string), typeof(ChecklistMetaEditor), string.Empty, BindingMode.TwoWay);
	public string Name
	{
		get => (string)GetValue(NameProperty);
		set => SetValue(NameProperty, value);
	}
	public static readonly BindableProperty UseDeadlineProperty = BindableProperty.Create(nameof(UseDeadline), typeof(bool), typeof(ChecklistMetaEditor), false, BindingMode.TwoWay);
	public bool UseDeadline
	{
		get => (bool)GetValue(UseDeadlineProperty);
		set => SetValue(UseDeadlineProperty, value);
	}
	public static readonly BindableProperty DeadlineProperty = BindableProperty.Create(nameof(Deadline), typeof(DateTime), typeof(ChecklistMetaEditor), DateTime.Now, BindingMode.TwoWay);
	public DateTime Deadline
	{
		get => (DateTime)GetValue(DeadlineProperty);
		set => SetValue(DeadlineProperty, value);
	}
	public static readonly BindableProperty ChecklistColorProperty = BindableProperty.Create(nameof(ChecklistColor), typeof(Checklist.ChecklistColor), typeof(ChecklistMetaEditor), Checklist.ChecklistColor.Magenta, BindingMode.TwoWay, propertyChanged:OnChecklistColorChanged);

    public Checklist.ChecklistColor ChecklistColor
	{
		get => (Checklist.ChecklistColor)GetValue(ChecklistColorProperty);
		set => SetValue(ChecklistColorProperty, value);
    }

    private static void OnChecklistColorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var editor = (ChecklistMetaEditor)bindable;
		editor.OnPropertyChanged(nameof(ChecklistColor));
    }

    public ChecklistMetaEditor()
	{
		InitializeComponent();
	}
}