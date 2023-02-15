namespace ChatApp.Mobile.Models;

public class Group : GroupWithoutEntities
{
    public MessageWithoutEntities LastMessage => Messages?
        .OrderBy(m => m.SentAt)
        .Last();
}
