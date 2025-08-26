using System.Globalization;

namespace Tokero.Converters
{
    public class RankToPerformanceTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int rank)
            {
                return rank switch
                {
                    1 => "🥇 Champion!",
                    2 => "🥈 Runner-up!",
                    3 => "🥉 Bronze!",
                    <= 5 => "Top 5!",
                    <= 10 => "Top 10!",
                    <= 20 => "Above Average",
                    <= 50 => "Average",
                    _ => "Room to Improve"
                };
            }
            return "Unknown";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
