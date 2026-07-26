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
            roundedSlider.OnValueChanged();
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
        Slider.TranslationX = Math.Clamp(Slider.TranslationX + e.TotalX, 0, AbsoluteLayout.Width - Slider.Width);
        Value = GetValueFromTranslation();
    }

    public void OnValueChanged()
    {
        Slider.TranslationX = GetTranslationFromValue();
    }

    private double GetTranslationFromValue()
    {
        return ((Value - Min) / (Max - Min)) * (AbsoluteLayout.Width - Slider.Width);
    }

    private double GetValueFromTranslation()
    {
        return ((Slider.TranslationX / (AbsoluteLayout.Width - Slider.Width)) * (Max - Min)) + Min;
    }
}