using ChecklistApp.Model;
using ChecklistApp.Services;
using Microsoft.Maui.Controls.Internals;

namespace ChecklistApp.MarkupExtensions;

[RequireService([typeof(IProvideValueTarget), typeof(IReferenceProvider), typeof(ColorService)])]
public class DynamicChecklistColorResourceExtension : BindableObject, IMarkupExtension<BindingBase>
{
    public static readonly BindableProperty ColorProperty = BindableProperty.Create(nameof(Color), typeof(Checklist.ChecklistColor), typeof(DynamicChecklistColorResourceExtension));
    public static readonly BindableProperty CustomColorIdProperty = BindableProperty.Create(nameof(CustomColorId), typeof(int?), typeof(DynamicChecklistColorResourceExtension));

    private ColorService? _colorService = null;
    
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

    public bool FlipFLop { get; set; } = false;

    public BindingBase ProvideValue(IServiceProvider serviceProvider)
    {
        Application.Current.Resources.TryGetValue("ChecklistColorToColorConverter", out object converter);
        _colorService = IPlatformApplication.Current.Services.GetService<ColorService>();
        if (_colorService is not null)
        {
            _colorService.ColorsUpdated += ColorServiceOnColorsUpdated;
        }
        
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
                    source: this), 
                BindingBase.Create(static (DynamicChecklistColorResourceExtension c) => c.FlipFLop, BindingMode.OneWay, 
                    source: this)
            },
            Converter = (IMultiValueConverter)converter
        };
    }

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
    {
        return ProvideValue(serviceProvider);
    }

    private void ColorServiceOnColorsUpdated(object? sender, ColorsUpdatedEventArgs e)
    {
        if (e.AffectedIds.All(x => x != CustomColorId))
            return;
        
        FlipFLop = !FlipFLop;
        OnPropertyChanged(nameof(FlipFLop));
    }
}