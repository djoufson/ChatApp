using System.Globalization;

namespace ChatApp.Mobile.Converters;

public class ColumnConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string fromUserEmail)
            return (fromUserEmail.Equals(Preferences.Get(Constants.USER_EMAIL_KEY, ""))) ?
                1 :
                0;

        return 1;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return null;
    }
}
