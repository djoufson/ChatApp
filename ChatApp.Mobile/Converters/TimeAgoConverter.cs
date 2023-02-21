namespace ChatApp.Mobile.Converters;

public class TimeAgoConverter : IValueConverter
{
    const int SECOND = 1;
    const int MINUTE = 60 * SECOND;
    const int HOUR = 60 * MINUTE;
    const int DAY = 24 * HOUR;
    const int MONTH = 30 * DAY;
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var yourDate = (DateTime)value;
        var ts = new TimeSpan(DateTime.UtcNow.Ticks - yourDate.ToUniversalTime().Ticks);
        double delta = Math.Abs(ts.TotalSeconds);

        if (delta < 1 * MINUTE)
            return ts.Seconds <= 2 ? "just now" : ts.Seconds + "s ago";

        if (delta < 2 * MINUTE)
            return "a minute ago";

        if (delta < 45 * MINUTE)
            return ts.Minutes + " minutes ago";

        if (delta < 90 * MINUTE)
            return "an hour ago";

        if (delta < 24 * HOUR)
            return ts.Hours + " hours ago";

        if (delta < 48 * HOUR)
            return "yesterday";

        if (delta < 30 * DAY)
            return ts.Days + " days ago";

        if (delta < 12 * MONTH)
        {
            int months = (int)Math.Floor((double)ts.Days / 30);
            return months <= 1 ? "last month" : months + " months ago";
        }
        else
        {
            int years = (int)Math.Floor((double)ts.Days / 365);
            return years <= 1 ? "last year" : years + " years ago";
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return null;
    }
}
