using System.Globalization;

namespace Tokero.Converters
{
    public class StringContainsConverter : IValueConverter
    {
        public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if(value is IEnumerable<string> stringCollection && parameter is string searchString)
                {
                    var result = stringCollection.Contains(searchString);
                    return result;
                }
                return false;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}