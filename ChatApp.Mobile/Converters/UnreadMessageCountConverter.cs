namespace ChatApp.Mobile.Converters;

internal class UnreadMessageCountConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if(value is not Conversation conversation)
            return 0;

        if (conversation.LastMessage.ToUserName == App.UserName)
        {
            return conversation.UnreadMessagesCount < 10 ?
                conversation.UnreadMessagesCount :
                "+9";
        }
        return 0;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return null;
    }
}
