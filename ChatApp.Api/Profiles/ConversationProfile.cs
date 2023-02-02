namespace ChatApp.Api.Profiles;

public class ConversationProfile : Profile
{
    public ConversationProfile()
    {
        // source -> target
        CreateMap<Conversation, ConversationDto>();
        CreateMap<ConversationDto, Conversation>();
        CreateMap<ConversationDto, ConversationWithoutEntities>();
    }
}
