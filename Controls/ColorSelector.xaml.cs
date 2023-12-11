namespace ChecklistApp.Controls;
using ChecklistApp.Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

public partial class ColorSelector : ContentView
{
	public static readonly BindableProperty SelectedColorProperty = BindableProperty.Create(nameof(SelectedColor), typeof(Checklist.ChecklistColor), typeof(ColorSelector), Checklist.ChecklistColor.Red, BindingMode.TwoWay, propertyChanged:OnSelectedColorChanged);

    public Checklist.ChecklistColor SelectedColor
	{
		get => (Checklist.ChecklistColor)GetValue(SelectedColorProperty);
		set => SetValue(SelectedColorProperty, value);
    }

    private static void OnSelectedColorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var selector = (ColorSelector)bindable;
        selector.OnPropertyChanged(nameof(SelectedColor));
        selector.OnPropertyChanged(nameof(GreyChecked));
        selector.OnPropertyChanged(nameof(CyanChecked));
        selector.OnPropertyChanged(nameof(BlueChecked));
        selector.OnPropertyChanged(nameof(PurpleChecked));
        selector.OnPropertyChanged(nameof(MagentaChecked));
        selector.OnPropertyChanged(nameof(RedChecked));
        selector.OnPropertyChanged(nameof(OrangeChecked));
        selector.OnPropertyChanged(nameof(GreenChecked));
    }

    public bool GreyChecked
	{
		get
		{
			return SelectedColor.Equals(Checklist.ChecklistColor.Grey);
		}
	}

	public bool CyanChecked
	{
		get
		{
			return SelectedColor.Equals(Checklist.ChecklistColor.Cyan);
		}
	}

    public bool BlueChecked
    {
        get
        {
            return SelectedColor.Equals(Checklist.ChecklistColor.Blue);
        }
    }

    public bool PurpleChecked
    {
        get
        {
            return SelectedColor.Equals(Checklist.ChecklistColor.Purple);
        }
    }

    public bool MagentaChecked
    {
        get
        {
            return SelectedColor.Equals(Checklist.ChecklistColor.Magenta);
        }
    }

    public bool RedChecked
    {
        get
        {
            return SelectedColor.Equals(Checklist.ChecklistColor.Red);
        }
    }

    public bool OrangeChecked
    {
        get
        {
            return SelectedColor.Equals(Checklist.ChecklistColor.Orange);
        }
    }

    public bool GreenChecked
    {
        get
        {
            return SelectedColor.Equals(Checklist.ChecklistColor.Green);
        }
    }

    public ICommand SetColorCommand { get; set; }

    public ColorSelector()
    {
        SetColorCommand = new Command<string>(SetColor);
        InitializeComponent();
	}

    private void SetColor(string value)
    {
        if (value is null)
            return;

        Checklist.ChecklistColor color = (Checklist.ChecklistColor)Enum.Parse(typeof(Checklist.ChecklistColor), value);
        SelectedColor = color;
        OnPropertyChanged(nameof(GreyChecked));
        OnPropertyChanged(nameof(CyanChecked));
        OnPropertyChanged(nameof(BlueChecked));
        OnPropertyChanged(nameof(PurpleChecked));
        OnPropertyChanged(nameof(MagentaChecked));
        OnPropertyChanged(nameof(RedChecked));
        OnPropertyChanged(nameof(OrangeChecked));
        OnPropertyChanged(nameof(GreenChecked));
    }
}