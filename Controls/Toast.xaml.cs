using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChecklistApp.Controls;

public partial class Toast : ContentView
{
    public static readonly BindableProperty BaseSpacingProperty = BindableProperty.Create(nameof(BaseSpacing), typeof(Thickness), typeof(Toast), new Thickness(0, 0, 0, 60));
    
    public event EventHandler ToastCompleted;
    
    private string _message = "Cheers!";
    private bool _open = false;
    private double _popOutTarget = 0;

    public string Message
    {
        get => _message;
        set
        {
            _message = value;
            OnPropertyChanged();
        }
    }

    public Thickness BaseSpacing
    {
        get => (Thickness)GetValue(BaseSpacingProperty);
        set => SetValue(BaseSpacingProperty, value);
    }
    
    public Toast()
    {
        InitializeComponent();
        this.SizeChanged += (sender, args) =>
        {
            Box.TranslationY = this.Height - Box.Y;
        };
    }

    private async Task PopIn()
    {
        _open = true;
        await Box.TranslateTo(Box.TranslationX, _popOutTarget, easing: Easing.SpringOut);
        await Task.Delay(5000).ContinueWith(_ => PopOut());
    }

    private async Task PopOut()
    {
        _open = false;
        await Box.TranslateTo(Box.TranslationX, @this.Height - Box.Y, easing: Easing.SpringIn);
        ToastCompleted?.Invoke(this, EventArgs.Empty);
    }

    public void PerformToast(string message)
    {
        Message = message;
        PopIn();
    }

    public void AdjustToNewAnchor(Rect anchor)
    {
        //  If space below, attach to bottom
        if (this.Height - (anchor.Y + anchor.Height) >= Box.Height + Box.Margin.VerticalThickness)
            _popOutTarget = anchor.Y + anchor.Height + Box.Margin.Top - Box.Y;
        //  Else attach to top
        else
            _popOutTarget = anchor.Y - Box.Y - (Box.Height + Box.Margin.Bottom);

        if (!_open)
            return;
        
        Box.TranslateTo(Box.TranslationX, _popOutTarget, easing: Easing.CubicInOut);
    }

    public void ResetAnchoring()
    {
        _popOutTarget = 0;

        if (!_open)
            return;
        
        Box.TranslateTo(Box.TranslationX, _popOutTarget, easing: Easing.CubicInOut);
    }
}