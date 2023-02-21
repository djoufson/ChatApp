namespace ChatApp.Mobile.Converters;

internal class IsUnreadTagVisible : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not Conversation conversation)
            return false;

        if (conversation.LastMessage.ToUserName == App.UserName)
            return conversation.UnreadMessagesCount > 0;

        return false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
