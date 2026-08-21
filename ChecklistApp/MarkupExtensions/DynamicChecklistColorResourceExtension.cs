using ChecklistApp.Model;
using ChecklistApp.Services;
using Microsoft.Maui.Controls.Internals;

namespace ChecklistApp.MarkupExtensions;

[RequireService([typeof(IProvideValueTarget), typeof(IReferenceProvider), typeof(ColorService)])]
public class DynamicChecklistColorResourceExtension : BindableObject, IMarkupExtension<BindingBase>
{
    public static readonly BindableProperty ColorProperty = BindableProperty.Create(nameof(Color), typeof(Checklist.ChecklistColor?), typeof(DynamicChecklistColorResourceExtension), propertyChanged:BindingsChanged, defaultValue:null);
    public static readonly BindableProperty CustomColorIdProperty = BindableProperty.Create(nameof(CustomColorId), typeof(int?), typeof(DynamicChecklistColorResourceExtension), propertyChanged:BindingsChanged, defaultValue:null);

    private static void BindingsChanged(BindableObject bindable, object oldValue, object newValue)
    {
        ((DynamicChecklistColorResourceExtension)bindable).OnBindingsChanged();
    }

    private ColorService? _colorService = null;
    private Color _resource = new Color(0);
    
    public Checklist.ChecklistColor? Color
    {
        get => (Checklist.ChecklistColor)GetValue(ColorProperty);
        set => SetValue(ColorProperty, value);
    }

    public int? CustomColorId
    {
        get => (int?)GetValue(CustomColorIdProperty);
        set => SetValue(CustomColorIdProperty, value);
    }

    public Color Resource
    {
        get => _resource;
        set
        {
            if (!_resource.Equals(value))
            {
                _resource = value;
                OnPropertyChanged(nameof(Resource));
            }
        }
    }

    public BindingBase ProvideValue(IServiceProvider serviceProvider)
    {
        if (Application.Current.Resources.TryGetValue("Foreground", out var res) && res is Color col)
            Resource = col;
        
        _colorService = IPlatformApplication.Current.Services.GetService<ColorService>();
        if (_colorService is not null)
        {
            _colorService.ColorsUpdated += ColorServiceOnColorsUpdated;
        }
        
        if (serviceProvider.GetService(typeof(IProvideValueTarget)) is IProvideValueTarget provideValueTarget && provideValueTarget.TargetObject is BindableObject targetObject)
        {
            this.SetBinding(BindingContextProperty, static (BindableObject b) => b.BindingContext, BindingMode.OneWay, source: targetObject);
        }
        
        return new Binding(nameof(Resource), BindingMode.OneWay, source: this);
    }

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
    {
        return ProvideValue(serviceProvider);
    }

    private void ColorServiceOnColorsUpdated(object? sender, ColorsUpdatedEventArgs e)
    {
        if (e.AffectedIds.All(x => x != CustomColorId))
            return;
        
        ResolveColourResource();
    }

    public void OnBindingsChanged()
    {
        ResolveColourResource();
    }

    private void ResolveColourResource()
    {
        if (Color is null)
            return;
        if (Application.Current.Resources.TryGetValue(ColorService.CalculateResourceKey((Checklist.ChecklistColor)Color, CustomColorId),
                out object resource) && resource is Color color)
        {
            Resource = color;
        }
        else if (Application.Current.Resources.TryGetValue("Foreground", out resource) && resource is Color defCol)
        {
            Resource = defCol;
        }
        else
        {
            throw new Exception("Shit's fucked yo");
        }
    }
}