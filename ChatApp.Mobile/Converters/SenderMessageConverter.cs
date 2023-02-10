using System.Globalization;

namespace ChatApp.Mobile.Converters;

public class SenderMessageConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string fromUserName)
            return (fromUserName.Equals(App.UserName)) ?
                "You" :
                fromUserName;

        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return string.Empty;
    }
}
