namespace ChatApp.Mobile.Converters;

internal class SenderMessageSpecialConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string fromUserName)
            return (fromUserName.Equals(App.UserName)) ?
                "You : " :
                String.Empty;

        return String.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return String.Empty;
    }
}
