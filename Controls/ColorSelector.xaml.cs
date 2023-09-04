namespace ChecklistApp.Controls;
using ChecklistApp.Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

public partial class ColorSelector : ContentView, INotifyPropertyChanged
{
	public static readonly BindableProperty ColorProperty = BindableProperty.Create(nameof(Color), typeof(Checklist.ChecklistColor), typeof(ColorSelector), Checklist.ChecklistColor.Grey, BindingMode.TwoWay);
	public Checklist.ChecklistColor Color
	{
		get => (Checklist.ChecklistColor)GetValue(ColorProperty);
		set => SetValue(ColorProperty, value);
	}

	public bool GreyChecked
	{
		get
		{
			return Color.Equals(Checklist.ChecklistColor.Grey);
		}
	}

	public bool CyanChecked
	{
		get
		{
			return Color.Equals(Checklist.ChecklistColor.Cyan);
		}
	}

    public bool BlueChecked
    {
        get
        {
            return Color.Equals(Checklist.ChecklistColor.Blue);
        }
    }

    public bool PurpleChecked
    {
        get
        {
            return Color.Equals(Checklist.ChecklistColor.Purple);
        }
    }

    public bool MagentaChecked
    {
        get
        {
            return Color.Equals(Checklist.ChecklistColor.Magenta);
        }
    }

    public bool RedChecked
    {
        get
        {
            return Color.Equals(Checklist.ChecklistColor.Red);
        }
    }

    public bool OrangeChecked
    {
        get
        {
            return Color.Equals(Checklist.ChecklistColor.Orange);
        }
    }

    public bool GreenChecked
    {
        get
        {
            return Color.Equals(Checklist.ChecklistColor.Grey);
        }
    }

    public ICommand SetColorCommand { get; set; }

    public ColorSelector()
    {
        SetColorCommand = new Command<string>(SetColor);
        InitializeComponent();
	}

    private void SetColorGrey()
    {
        SetColor("Grey");
    }

    private void SetColor(string value)
    {
        if (value is null)
            return;

        Checklist.ChecklistColor color = (Checklist.ChecklistColor)Enum.Parse(typeof(Checklist.ChecklistColor), value);
        Color = color;
        OnPropChange(nameof(GreyChecked));
        OnPropChange(nameof(CyanChecked));
        OnPropChange(nameof(BlueChecked));
        OnPropChange(nameof(PurpleChecked));
        OnPropChange(nameof(MagentaChecked));
        OnPropChange(nameof(RedChecked));
        OnPropChange(nameof(OrangeChecked));
        OnPropChange(nameof(GreenChecked));
    }

    public event PropertyChangedEventHandler PropChange = (sender, e) => { };

    public void OnPropChange([CallerMemberName] string property = null)
    {
        PropChange(this, new PropertyChangedEventArgs(property));
    }
}