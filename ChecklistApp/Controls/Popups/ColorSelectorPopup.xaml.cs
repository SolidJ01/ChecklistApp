using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChecklistApp.Controls;

public partial class ColorSelectorPopup : Popup
{
    public static readonly BindableProperty ColorProperty = BindableProperty.Create(nameof(Color), typeof(Color), typeof(ColorSelectorPopup), Microsoft.Maui.Graphics.Color.FromArgb("#ffffff"));

    private double _hue = 0;
    private double _saturation = 0;
    private double _value = 0;
    
    public Color Color
    {
        get => (Color)GetValue(ColorProperty);
        set => SetValue(ColorProperty, value);
    }

    public double Hue
    {
        get => _hue;
        set
        {
            _hue = value;
            UpdateColor();
        }
    }

    public double Saturation
    {
        get => _saturation;
        set
        {
            _saturation = value;
            UpdateColor();
        }
    }

    public double Value
    {
        get => _value;
        set
        {
            _value = value;
            UpdateColor();
        }
    }
    
    public ColorSelectorPopup()
    {
        InitializeComponent();
    }

    private void UpdateColor()
    {
        Color = Color.FromHsv((float)_hue, (float)_saturation, (float)_value);
        OnPropertyChanged(nameof(Color));
    }

    private void UpdateHSV()
    {
        float oldHue = Color.GetHue();
        float oldSaturation = Color.GetSaturation();
        float oldLuminosity = Color.GetLuminosity();
        float newHue = oldHue;
        float newValue = oldLuminosity + oldSaturation * Math.Min(oldLuminosity, 1 - oldLuminosity);
        float newSaturation = newValue == 0 ? 0f : (float)(2 * (1 - oldLuminosity / newValue));
        Hue = newHue;
        Saturation = newSaturation;
        Value = newValue;
        OnPropertyChanged(nameof(Hue));
        OnPropertyChanged(nameof(Saturation));
        OnPropertyChanged(nameof(Value));
    }
}