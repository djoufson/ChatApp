namespace ChatApp.Mobile.Extensions;

public static class UserExtensions
{
    public static UserWithoutEntities DeepCopy(this UserWithoutEntities target, UserWithoutEntities source)
    {
        target.Id = source.Id;
        target.UserName = source.UserName;
        target.Email = source.Email;
        target.PhoneNumber = source.PhoneNumber;
        target.LastTimeOnline = source.LastTimeOnline;
        target.Conversations = source.Conversations;
        return target;
    }
}
