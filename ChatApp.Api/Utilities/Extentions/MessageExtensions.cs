namespace ChatApp.Api.Utilities.Extentions;

public static class MessageExtensions
{
    public static MessageDto AsDto(this Message self, IMapper mapper)
    {
        return mapper.Map<MessageDto>(self);
    }

    public static MessageWithoutEntities WithoutEntities(this Message self, IMapper mapper)
    {
        var messageDto = mapper.Map<MessageDto>(self);
        return mapper.Map<MessageWithoutEntities>(messageDto);
    }

    public static MessageWithoutEntities WithoutEntities(this MessageDto self, IMapper mapper)
    {
        return mapper.Map<MessageWithoutEntities>(self);
    }
}
