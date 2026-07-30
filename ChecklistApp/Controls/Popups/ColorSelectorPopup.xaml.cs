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
        if (Application.Current.Resources.TryGetValue("Foreground", out var resource) && resource is Color color)
        {
            Color = color;
            OnPropertyChanged(nameof(Color));
            UpdateHsv();
        }
    }

    private void UpdateColor()
    {
        Color = Color.FromHsv((float)_hue, (float)_saturation, (float)_value);
        OnPropertyChanged(nameof(Color));
    }

    private void UpdateHsv()
    {
        float xMax = Math.Max(Math.Max(Color.Red, Color.Green),  Color.Blue);
        float xMin = Math.Min(Math.Min(Color.Red, Color.Green), Color.Blue);
        float chroma = xMax - xMin;
        float newHue =
            chroma == 0
                ? 0
                : xMax == Color.Red
                    ? (((Color.Green - Color.Blue) / chroma) % 6)
                    : xMax == Color.Green
                        ? ((Color.Blue - Color.Red) / chroma + 2)
                        : ((Color.Red - Color.Green) / chroma + 4);
        float newSaturation = xMax == 0 ? 0 : chroma / xMax;
        _hue = newHue;
        _saturation = newSaturation;
        _value = xMax;
        OnPropertyChanged(nameof(Hue));
        OnPropertyChanged(nameof(Saturation));
        OnPropertyChanged(nameof(Value));
    }
}