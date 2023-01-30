namespace ChatApp.Api.Profiles;

public class GroupsProfile : Profile
{
	public GroupsProfile()
	{
        // source -> destination

        CreateMap<Group, GroupDto>();
        CreateMap<GroupDto, GroupWithoutEntities>();
        CreateMap<GroupDto, Group>().ConstructUsing(src =>
            new Group()
            {
                Id = src.Id,
                Name = src.Name,
            }
        );
    }
}
