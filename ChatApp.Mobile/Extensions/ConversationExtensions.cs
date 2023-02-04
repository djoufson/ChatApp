namespace ChatApp.Mobile.Extensions;

public static class ConversationExtensions
{
    public static Conversation AsConversation(this ConversationWithoutEntities self)
    {
        return new Conversation()
        {
            Id = self.Id,
            Messages = self.Messages
        };
    }
}
