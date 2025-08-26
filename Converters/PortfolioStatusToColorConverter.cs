using System.Globalization;

namespace Tokero.Converters
{
    public class PortfolioStatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isInPortfolio)
            {
                return isInPortfolio ? "✓" : "✗";
            }
            return "✗";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
