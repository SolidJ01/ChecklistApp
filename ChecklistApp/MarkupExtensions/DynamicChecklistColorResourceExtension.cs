using ChecklistApp.Model;
using ChecklistApp.Services;
using Microsoft.Maui.Controls.Internals;

namespace ChecklistApp.MarkupExtensions;

[RequireService([typeof(IProvideValueTarget), typeof(IReferenceProvider)])]
public class DynamicChecklistColorResourceExtension : BindableObject, IMarkupExtension<BindingBase>
{
    public static readonly BindableProperty ColorProperty = BindableProperty.Create(nameof(Color), typeof(Checklist.ChecklistColor), typeof(DynamicChecklistColorResourceExtension));
    public static readonly BindableProperty CustomColorIdProperty = BindableProperty.Create(nameof(CustomColorId), typeof(int?), typeof(DynamicChecklistColorResourceExtension));

    public Checklist.ChecklistColor Color
    {
        get => (Checklist.ChecklistColor)GetValue(ColorProperty);
        set => SetValue(ColorProperty, value);
    }

    public int? CustomColorId
    {
        get => (int?)GetValue(CustomColorIdProperty);
        set => SetValue(CustomColorIdProperty, value);
    }

    public BindingBase ProvideValue(IServiceProvider serviceProvider)
    {
        Application.Current.Resources.TryGetValue("ChecklistColorToColorConverter", out object converter);
        
        if (serviceProvider.GetService(typeof(IProvideValueTarget)) is IProvideValueTarget provideValueTarget && provideValueTarget.TargetObject is BindableObject targetObject)
        {
            this.SetBinding(BindingContextProperty, static (BindableObject b) => b.BindingContext, BindingMode.OneWay, source: targetObject);
        }
        
        return new MultiBinding
        {
            Bindings =
            {
                BindingBase.Create(static (DynamicChecklistColorResourceExtension c) => c.Color, BindingMode.OneWay,
                    source: this),
                BindingBase.Create(static (DynamicChecklistColorResourceExtension c) => c.CustomColorId, BindingMode.OneWay,
                    source: this)
            },
            Converter = (IMultiValueConverter)converter
        };
    }

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
    {
        return ProvideValue(serviceProvider);
    }
}