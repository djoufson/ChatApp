namespace ChatApp.Api.Profiles;

public class GroupsProfile : Profile
{
	public GroupsProfile()
	{
        // source -> destination

        CreateMap<Group, GroupDto>();
        CreateMap<GroupDto, Group>();
    }
}
