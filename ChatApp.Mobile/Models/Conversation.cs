using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ChatApp.Mobile.Models;

public class Conversation : ConversationWithoutEntities, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    public MessageWithoutEntities LastMessage => Messages?
        .OrderBy(m => m.SentAt)
        .Last();

    public string ToUserEmail 
    {
        get
        {
            var email1 = LastMessage.ToUserEmail;
            var email2 = LastMessage.FromUserEmail;
            var userMail = App.UserEmail;

            if (userMail.Equals(email1, StringComparison.OrdinalIgnoreCase))
                return email2;

            return email1;
        }
    }
    public string ToUserName
    {
        get
        {
            var username1 = LastMessage.ToUserName;
            var username2 = LastMessage.FromUserName;
            var username = App.UserName;

            if (username.Equals(username1, StringComparison.OrdinalIgnoreCase))
                return username2;

            return username1;
        }
    }

    public void OnPropertyChanged(string propertyName = nameof(LastMessage))
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
