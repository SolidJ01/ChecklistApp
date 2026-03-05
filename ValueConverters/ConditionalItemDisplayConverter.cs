using System.Globalization;

namespace ChecklistApp.ValueConverters;

public class ConditionalItemDisplayConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        var check = values.FirstOrDefault(x => x is bool);
        if (check is not bool b || b) return values.First(x => x is not bool);
        
        var hide = values.First(x => x is not bool);
        
        return Activator.CreateInstance(hide.GetType());
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        return null;
    }
}