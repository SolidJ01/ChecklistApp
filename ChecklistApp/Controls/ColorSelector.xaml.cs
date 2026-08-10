using System.Collections.ObjectModel;
using System.Collections.Specialized;
using ChecklistApp.Model;
using ChecklistApp.ViewModel;

namespace ChecklistApp.Controls;

public partial class ColorSelector : ContentView
{
	public static readonly BindableProperty SelectableColorsProperty = BindableProperty.Create(nameof(SelectableColors), typeof(ObservableCollection<SelectableColorViewModel>),  typeof(ColorSelector), new ObservableCollection<SelectableColorViewModel>(), propertyChanged:SelectableColorsPropertyChanged);

	private static void SelectableColorsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is ColorSelector selector)
			selector.RedrawUI();
	}

	public ObservableCollection<SelectableColorViewModel> SelectableColors
	{
		get => (ObservableCollection<SelectableColorViewModel>)GetValue(SelectableColorsProperty);
		set => SetValue(SelectableColorsProperty, value);
	}

    public ColorSelector()
    {
        InitializeComponent();
	}

    private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
	    foreach (var item in e.NewItems)
	    {
		    SelectableColorViewModel viewModel = item as SelectableColorViewModel;
		    var button = CreateButton(viewModel);
		    Layout.Add(button);
	    }
    }

    public void RedrawUI()
    {
	    SelectableColors.CollectionChanged += OnCollectionChanged;
	    for (int i = 0; i < SelectableColors.Count; i++)
	    {
		    var item = SelectableColors[i];
		    Layout.Add(CreateButton(item), i % 5, i / 5);
	    }
    }

    private ColorButton CreateButton(SelectableColorViewModel viewModel)
    {
	    Application.Current.Resources.TryGetValue("ChecklistColorConverter", out var checklistColorConverter);
	    Application.Current.Resources.TryGetValue("DynamicCommandConverter", out var dynamicCommandConverter);
	    Application.Current.Resources.TryGetValue("BoolToInt", out var boolToIntConverter);
	    
	    var button = new ColorButton();
	    button.BindingContext = viewModel;
	    
	    button.SetBinding(ColorButton.BackgroundBrushProperty, new MultiBinding
	    {
		    Bindings = [new Binding(nameof(viewModel.Color)), new Binding(nameof(viewModel.CustomColorId))], 
		    Converter = (IMultiValueConverter)checklistColorConverter
	    });
	    
	    button.SetBinding(ColorButton.IsCheckedProperty, new Binding
	    {
		    Path = nameof(viewModel.Selected)
	    });
	    
	    button.SetBinding(ColorButton.CommandProperty, new MultiBinding
	    {
		    Bindings = [new Binding(nameof(viewModel.Command)), new Binding(nameof(viewModel.SelectedCommand))],
		    Converter = (IMultiValueConverter)dynamicCommandConverter,
		    ConverterParameter = new Binding(nameof(viewModel.Selected), converter: (IValueConverter)boolToIntConverter)
	    });
	    
	    button.SetBinding(ColorButton.CommandParameterProperty, new Binding(viewModel.Color == Checklist.ChecklistColor.Custom ? nameof(viewModel.CustomColorId) : nameof(viewModel.Color)));
	    
	    return button;
    }
}