using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChecklistApp.Model;
using Color = Microsoft.Maui.Graphics.Color;
using Size = Microsoft.Maui.Graphics.Size;

namespace ChecklistApp.Controls;

public partial class ColorSelectorPopup : Popup
{
    public static readonly BindableProperty ColorProperty = BindableProperty.Create(nameof(Color), typeof(Color), typeof(ColorSelectorPopup), Microsoft.Maui.Graphics.Color.FromArgb("#ffffff"));
    public static readonly BindableProperty BackgroundDimensionsProperty = BindableProperty.Create(nameof(BackgroundDimensions), typeof(Size), typeof(ColorSelectorPopup), default(Size), BindingMode.OneWayToSource, propertyChanged:BackgroundDimensionsChanged);

    private static void BackgroundDimensionsChanged(BindableObject bindable, object oldValue, object newValue)
    {
        ColorSelectorPopup popup = (ColorSelectorPopup)bindable;
        popup.RegenerateHueImage();
        //popup.RegenerateSaturationImage();
    }

    private double _hue = 0;
    private double _saturation = 0;
    private double _value = 0;
    
    public Color Color
    {
        get => (Color)GetValue(ColorProperty);
        set => SetValue(ColorProperty, value);
    }

    public Color SaturatedLitColor { get; set; }

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

    public Size BackgroundDimensions
    {
        get => (Size)GetValue(BackgroundDimensionsProperty);
        set => SetValue(BackgroundDimensionsProperty, value);
    }

    public ImageSource HueSelectorBackground { get; set; }
    public ImageSource SaturationSelectorBackground { get; set; }
    public ImageSource ValueSelectorBackground { get; set; }
    
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
        SaturatedLitColor = Color.FromHsv((float)_hue, 1, 1);
        OnPropertyChanged(nameof(Color));
        OnPropertyChanged(nameof(SaturatedLitColor));
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

    public void RegenerateHueImage()
    {
        int width = (int)BackgroundDimensions.Width;
        int height = (int)BackgroundDimensions.Height;
        
        Bitmap bitmap = new Bitmap(width, height);
        for (int x = 0; x < width; x++)
        {
            Color color = Color.FromHsv(x / (float)width, 1f, 1f);
            for (int y = 0; y < height; y++)
            {
                bitmap.MapPixel(x, y, color);
            }
        }

        byte[] byteArray = bitmap.AsByteArray();
        
        MemoryStream memoryStream = new MemoryStream(byteArray);
        HueSelectorBackground = ImageSource.FromStream(() => memoryStream);
        OnPropertyChanged(nameof(HueSelectorBackground));
    }

    public void RegenerateSaturationImage()
    {
        int width = (int)BackgroundDimensions.Width;
        int height = (int)BackgroundDimensions.Height;
        
        Bitmap bitmap = new Bitmap(width, height);
        for (int x = 0; x < width; x++)
        {
            Color color = Color.FromHsva((float)Hue, 0, 1f, 0);
            for (int y = 0; y < height; y++)
            {
                bitmap.MapPixel(x, y, color);
            }
        }
        
        MemoryStream memoryStream = new MemoryStream(bitmap.AsByteArray());
        SaturationSelectorBackground = ImageSource.FromStream(() => memoryStream);
        OnPropertyChanged(nameof(SaturationSelectorBackground));
    }
}