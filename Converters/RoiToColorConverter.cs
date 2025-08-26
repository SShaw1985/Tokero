using System.Globalization;

namespace Tokero.Converters
{
    public class RoiToColorConverter : IValueConverter
    {
        public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is decimal roi)
            {
                if(roi > 0)
                    return Colors.Green;
                else if(roi < 0)
                    return Colors.Red;
                else
                    return Colors.Gray;
            }
            return Colors.Gray;
        }

        public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}