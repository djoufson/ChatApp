using System.Globalization;

namespace ChatApp.Mobile.Converters;

public class MessageSideAlignmentConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if(value is string fromUserEmail)
            return (fromUserEmail.Equals(Preferences.Get(Constants.USER_EMAIL_KEY, ""))) ?
                LayoutOptions.End :
                LayoutOptions.Start;
        
        return LayoutOptions.Start;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return string.Empty;
    }
}
