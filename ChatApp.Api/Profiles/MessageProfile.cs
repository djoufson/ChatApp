namespace ChatApp.Api.Profiles;

public class MessageProfile : Profile
{
	public MessageProfile()
	{
		// source -> target
		CreateMap<Message, MessageDto>();
		CreateMap<MessageDto, Message>();
	}
}
