using System.Globalization;

namespace ChecklistApp.ValueConverters;

public class DynamicCommandConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (parameter is int index && index >= 0 && index < values.Length)
        {
            return values[index];
        }
        else if (values.Length > 0)
            return values[0];
        else
            throw new Exception($"{nameof(values)} is not a valid parameter");
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}