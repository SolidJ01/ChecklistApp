using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChecklistApp.Model;

namespace ChecklistApp.ValueConverters
{
    public class ObservableItemCollectionToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.GetType() == typeof(ObservableCollection<Item>))
            {
                ObservableCollection<Item> items = (ObservableCollection<Item>)value;
                return items.Any();
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
