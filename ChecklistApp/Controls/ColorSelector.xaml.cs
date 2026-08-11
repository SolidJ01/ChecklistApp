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
	    if (e.OldItems != null)
	    {
		    foreach (var item in e.OldItems)
		    {
			    SelectableColorViewModel viewModel = item as SelectableColorViewModel;
			    ColorButton button = (ColorButton)Layout.Children[e.OldStartingIndex];
			    switch (e.Action)
			    {
				    case NotifyCollectionChangedAction.Move:
					    int newIndex = SelectableColors.IndexOf(viewModel);
					    Layout.SetColumn(button, newIndex % 5);
					    Layout.SetRow(button, newIndex / 5);
					    break;
				    case NotifyCollectionChangedAction.Remove:
					    Layout.Remove(button);
					    break;
			    }
		    }
	    }
	    if (e.NewItems is not null)
	    {
		    for (int i = e.NewStartingIndex; i < Layout.Children.Count; i++)
		    {
			    ColorButton button = (ColorButton)Layout.Children[i];
			    Layout.SetColumn(button, (i + e.NewItems.Count) % 5);
			    Layout.SetRow(button, (i + e.NewItems.Count) / 5);
		    }
		    for (int i = 0; i < e.NewItems.Count; i++)
		    {
			    var item = e.NewItems[i];
			    SelectableColorViewModel viewModel = item as SelectableColorViewModel;
			    var button = CreateButton(viewModel);
			    int index = e.NewStartingIndex + i;
			    Layout.Insert(index, button);
			    Layout.SetColumn(button, index % 5);
			    Layout.SetRow(button, index / 5);
		    }
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

	    button.SetBinding(ColorButton.CommandProperty, new Binding(nameof(viewModel.Command)));
	    button.SetBinding(ColorButton.SelectedCommandProperty, new Binding(nameof(viewModel.SelectedCommand)));
	    
	    button.SetBinding(ColorButton.CommandParameterProperty, new Binding(viewModel.Color == Checklist.ChecklistColor.Custom ? nameof(viewModel.CustomColorId) : nameof(viewModel.Color)));
	    
	    return button;
    }
}