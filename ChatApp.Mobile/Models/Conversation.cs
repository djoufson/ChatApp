namespace ChatApp.Mobile.Models;

public class Conversation : ConversationWithoutEntities
{
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
}
