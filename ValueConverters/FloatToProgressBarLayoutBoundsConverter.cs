using System.Globalization;

namespace ChecklistApp.ValueConverters;

public class FloatToProgressBarLayoutBoundsConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        float floatValue = (float)value;
        return new Rect(0, 0, floatValue, 1);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}