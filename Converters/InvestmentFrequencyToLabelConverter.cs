using System.Globalization;
using Tokero.Models;

namespace Tokero.Converters
{
    public class InvestmentFrequencyToLabelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is InvestmentFrequency frequency)
            {
                return frequency switch
                {
                    InvestmentFrequency.Weekly => "Invest on day of week",
                    InvestmentFrequency.Monthly => "Invest on day of month",
                    _ => "Invest on day"
                };
            }
            return "Invest on day";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
