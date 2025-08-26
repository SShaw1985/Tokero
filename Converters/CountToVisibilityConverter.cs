using System.Globalization;

namespace Tokero.Converters
{
    public class CountToVisibilityConverter : IValueConverter
    {
        public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is int count)
            {
                if(parameter is string paramStr && paramStr == "0")
                {
                    return count == 0;
                }
                return count > 0;
            }

            return false;
        }

        public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}