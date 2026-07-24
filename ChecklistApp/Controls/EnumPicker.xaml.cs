using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChecklistApp.Controls;

public partial class EnumPicker : ContentView
{
    public static readonly BindableProperty EnumProperty = BindableProperty.Create(nameof(Enum), typeof(Enum), typeof(EnumPicker), propertyChanged:EnumChanged, defaultBindingMode:BindingMode.TwoWay);

    private static void EnumChanged(BindableObject bindable, object oldValue, object newValue)
    {
        ((EnumPicker)bindable).OnEnumChanged((Enum)oldValue, (Enum)newValue);
    }

    public Enum Enum
    {
        get => (Enum)GetValue(EnumProperty);
        set => SetValue(EnumProperty, value);
    }

    public ObservableCollection<Enum> Values { get; set; } = [];
    
    public EnumPicker()
    {
        InitializeComponent();
    }

    public void OnEnumChanged(Enum oldValue, Enum newValue)
    {
        if (oldValue is not null && oldValue.GetType() == newValue.GetType())
            return;
        
        var values = Enum.GetValues(newValue.GetType());
        Values.Clear();
        foreach (var value in values)
            Values.Add((Enum)value);
        
        Picker.SelectedIndex = Values.IndexOf(newValue);
    }

    private void OnSelectedIndexChanged(object sender, EventArgs e)
    {
        Enum = (Enum)Picker.SelectedItem;
    }
}