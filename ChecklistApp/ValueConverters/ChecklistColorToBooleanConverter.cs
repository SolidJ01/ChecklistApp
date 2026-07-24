using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChecklistApp.Model;

namespace ChecklistApp.ValueConverters
{
    public class ChecklistColorToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Checklist.ChecklistColor colorvalue, colorparameter;
            colorvalue = (Checklist.ChecklistColor)value;
            colorparameter = (Checklist.ChecklistColor)parameter;
            bool result = colorvalue.Equals(colorparameter);
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
