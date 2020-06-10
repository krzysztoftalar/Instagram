using System;
using System.Globalization;

namespace DesktopUI.ValueConverters
{
    public class ValueToOppositeValueConverter : BaseValueConverter<ValueToOppositeValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return -(double?) value;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}