using System.Collections.ObjectModel;
using System.Collections.Specialized;
using ChecklistApp.Model;
using ChecklistApp.Services;
using ChecklistApp.ViewModel;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ChecklistApp.Controls;

public partial class ColorSelector : ContentView
{
	public static readonly BindableProperty SelectableColorsProperty = BindableProperty.Create(nameof(SelectableColors), typeof(ObservableCollection<SelectableColorViewModel>),  typeof(ColorSelector), new ObservableCollection<SelectableColorViewModel>(), propertyChanged:SelectableColorsPropertyChanged);
	public static readonly BindableProperty ColumnsProperty = BindableProperty.Create(nameof(Columns), typeof(int), typeof(ColorSelector), 5);

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

	public int Columns
	{
		get => (int)GetValue(ColumnsProperty);
		set => SetValue(ColumnsProperty, value);
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
					    Layout.SetColumn(button, newIndex % Columns);
					    Layout.SetRow(button, newIndex / Columns);
					    break;
				    case NotifyCollectionChangedAction.Remove:
					    Layout.Remove(button);
					    break;
			    }
		    }

		    for (int i = e.OldStartingIndex; i < SelectableColors.Count; i++)
		    {
			    ColorButton button = (ColorButton)Layout.Children[i];
			    Layout.SetColumn(button, i % Columns);
			    Layout.SetRow(button, i / Columns);
		    }

		    int nTargetRows = (int)Math.Round(Layout.Children.Count / (double)Columns, MidpointRounding.ToPositiveInfinity);
		    if (Layout.RowDefinitions.Count > nTargetRows)
		    {
			    for (int i = Layout.RowDefinitions.Count; i > nTargetRows; i--)
			    {
				    Layout.RowDefinitions.RemoveAt(i - 1);
			    }
		    }
	    }
	    if (e.NewItems is not null)
	    {
		    for (int i = e.NewStartingIndex; i < Layout.Children.Count; i++)
		    {
			    ColorButton button = (ColorButton)Layout.Children[i];
			    int targetRow = (int)Math.Round((i + e.NewItems.Count + 1) / (double)Columns, MidpointRounding.ToPositiveInfinity);
			    if (targetRow > Layout.RowDefinitions.Count)
			    {
				    
				    Layout.RowDefinitions.Add(new RowDefinition());
				    
			    }
			    
			    Layout.SetColumn(button, (i + e.NewItems.Count) % Columns);
			    Layout.SetRow(button, (i + e.NewItems.Count) / Columns);
		    }
		    for (int i = 0; i < e.NewItems.Count; i++)
		    {
			    var item = e.NewItems[i];
			    SelectableColorViewModel viewModel = item as SelectableColorViewModel;
			    var button = CreateButton(viewModel);
			    int index = e.NewStartingIndex + i;
			    Layout.Insert(index, button);
			    Layout.SetColumn(button, index % Columns);
			    Layout.SetRow(button, index / Columns);
		    }
	    }
    }

    public void RedrawUI()
    {
	    SelectableColors.CollectionChanged += OnCollectionChanged;
	    foreach (var child in Layout.Children)
	    {
		    Layout.Remove(child);
	    }

	    var columnDefs = new ColumnDefinitionCollection([]);
	    for (int i = 0; i < Columns; i++)
	    {
		    columnDefs.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
	    }

	    Layout.ColumnDefinitions = columnDefs;
	    for (int i = 0; i < SelectableColors.Count; i++)
	    {
		    var item = SelectableColors[i];
		    Layout.Add(CreateButton(item), i % Columns, i / Columns);
	    }
    }

    private ColorButton CreateButton(SelectableColorViewModel viewModel)
    {
	    Application.Current.Resources.TryGetValue("ChecklistColorConverter", out var checklistColorConverter);
	    Application.Current.Resources.TryGetValue("DynamicCommandConverter", out var dynamicCommandConverter);
	    Application.Current.Resources.TryGetValue("BoolToInt", out var boolToIntConverter);
	    
	    var button = new ColorButton();
	    button.BindingContext = viewModel;
	    
	    button.SetDynamicResource(ColorButton.BackgroundBrushProperty, ColorService.CalculateResourceKey(viewModel.Color, viewModel.CustomColorId));
	    // button.SetBinding(ColorButton.BackgroundBrushProperty, new MultiBinding
	    // {
		   //  Bindings = [new Binding(nameof(viewModel.Color)), new Binding(nameof(viewModel.CustomColorId))], 
		   //  Converter = (IMultiValueConverter)checklistColorConverter
	    // });
	    
	    button.SetBinding(ColorButton.IsCheckedProperty, new Binding
	    {
		    Path = nameof(viewModel.Selected)
	    });

	    button.SetBinding(ColorButton.CommandProperty, new Binding(nameof(viewModel.Command)));
	    button.SetBinding(ColorButton.SelectedCommandProperty, new Binding(nameof(viewModel.SelectedCommand)));
	    
	    button.SetBinding(ColorButton.CommandParameterProperty, new Binding(viewModel.Color == Checklist.ChecklistColor.Custom ? nameof(viewModel.CustomColorId) : nameof(viewModel.Color)));
	    
	    button.SetBinding(ColorButton.IconProperty, new Binding(nameof(viewModel.Icon)));
	    button.SetBinding(ColorButton.SmallIconProperty, new Binding(nameof(viewModel.SmallIcon)));
	    
	    return button;
    }
}