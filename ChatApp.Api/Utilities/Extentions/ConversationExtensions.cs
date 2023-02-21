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

    public static Conversation[] Sort(this Conversation[] self)
    {
        var list = self.ToList();
        list.Sort((c1, c2) => 
        {
            var last1 = c1.Messages?.Last();
            var last2 = c2.Messages?.Last();
            return (last1!.SentAt < last2!.SentAt) ? 1 : 0;
        });
        
        return list.ToArray();
    }
}
