using System.Globalization;

namespace ChecklistApp.ValueConverters;

public class ConditionalItemDisplayConverter : IValueConverter
{
    public object Convert(object values, Type targetType, object parameter, CultureInfo culture)
    {
        if (parameter is not bool or true) return values;

        return Activator.CreateInstance(targetType);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return null;
    }
}