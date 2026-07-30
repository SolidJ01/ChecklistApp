using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChecklistApp.Controls;

public partial class RoundedSlider : ContentView
{
    public static readonly BindableProperty MinProperty = BindableProperty.Create(nameof(Min), typeof(double), typeof(RoundedSlider), 0.0);
    public static readonly BindableProperty MaxProperty = BindableProperty.Create(nameof(Max), typeof(double), typeof(RoundedSlider), 100.0);
    public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(double), typeof(RoundedSlider), 0.0, BindingMode.TwoWay, propertyChanged: ValuePropertyChanged);

    private static void ValuePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is RoundedSlider roundedSlider)
        {
            roundedSlider.RecalculateSliderPosition();
        }
    }

    public double Min
    {
        get => (double)GetValue(MinProperty);
        set => SetValue(MinProperty, value);
    }

    public double Max
    {
        get => (double)GetValue(MaxProperty);
        set => SetValue(MaxProperty, value);
    }

    public double Value
    {
        get => (double)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }
    
    public RoundedSlider()
    {
        InitializeComponent();
    }

    private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
    {
        //Slider.TranslationX = Math.Clamp(Slider.TranslationX + e.TotalX, 0, AbsoluteLayout.Width - Slider.Width);
        Rect bounds = AbsoluteLayout.GetLayoutBounds((IView)Slider);
        bounds.X = Math.Clamp(bounds.X + e.TotalX, 0, AbsoluteLayout.Width - Slider.Width);
        AbsoluteLayout.SetLayoutBounds((IView)Slider, bounds);
        Value = GetValueFromTranslation();
    }

    public void RecalculateSliderPosition()
    {
        double translation = GetTranslationFromValue();
        if (!double.IsNaN(translation))
        {
            AbsoluteLayout.SetLayoutBounds((IView)Slider, new Rect(translation, 0, Slider.WidthRequest, Slider.HeightRequest));
        }
    }

    private double GetTranslationFromValue()
    {
        return ((Value - Min) / (Max - Min)) * (AbsoluteLayout.Width - Slider.Width);
    }

    private double GetValueFromTranslation()
    {
        Rect bounds = AbsoluteLayout.GetLayoutBounds((IView)Slider);
        return ((bounds.X / (AbsoluteLayout.Width - Slider.Width)) * (Max - Min)) + Min;
    }

    private void OnLayoutSizeChanged(object sender, EventArgs e)
    {
        RecalculateSliderPosition();
    }
}