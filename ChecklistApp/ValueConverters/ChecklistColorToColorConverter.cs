using System.Globalization;
using ChecklistApp.Model;
using ChecklistApp.Services;

namespace ChecklistApp.ValueConverters;

public class ChecklistColorToColorConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        Checklist.ChecklistColor color = Checklist.ChecklistColor.Grey;
        int? customColorId = null;
        foreach (var item in values)
        {
            if (item is Checklist.ChecklistColor colorItem)
                color = colorItem;
            else if (item is int intColor)
                customColorId = intColor;
        }

        string resourceKey = ColorService.CalculateResourceKey(color, customColorId);

        if (Application.Current.Resources.TryGetValue(resourceKey, out var resource))
        {
            return resource as Color;
        }
        else
        {
            throw new ArgumentException($"Resource {resourceKey} not found");
        }
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}