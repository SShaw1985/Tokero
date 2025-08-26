using System.Globalization;

namespace Tokero.Converters
{
    public class RankToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int rank)
            {
                return rank switch
                {
                    1 => Color.FromArgb("#FFD700"), // Gold
                    2 => Color.FromArgb("#C0C0C0"), // Silver
                    3 => Color.FromArgb("#CD7F32"), // Bronze
                    <= 5 => Color.FromArgb("#4F46E5"), // Indigo for top 5
                    <= 10 => Color.FromArgb("#6366F1"), // Purple for top 10
                    _ => Color.FromArgb("#6B7280") // Gray for others
                };
            }
            return Color.FromArgb("#6B7280"); // Default gray
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
