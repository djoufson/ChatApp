namespace ChatApp.Api.Utilities.Extentions;

public static class ConversationExtensions
{
    public static ConversationWithoutEntities WithoutEntities(this Conversation self, IMapper mapper)
    {
        var userDto = mapper.Map<ConversationDto>(self);
        return mapper.Map<ConversationWithoutEntities>(userDto);
    }

    public static ConversationWithoutEntities WithoutEntities(this ConversationDto self, IMapper mapper)
    {
        return mapper.Map<ConversationWithoutEntities>(self);
    }

    public static ConversationDto AsDto(this Conversation self, IMapper mapper)
    {
        return mapper.Map<ConversationDto>(self);
    }
}
