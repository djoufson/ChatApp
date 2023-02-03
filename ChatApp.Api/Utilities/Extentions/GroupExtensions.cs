namespace ChatApp.Api.Utilities.Extentions;

public static class GroupExtensions
{
    public static GroupDto AsDto(this Group self, IMapper mapper) 
    {
        return mapper.Map<GroupDto>(self);
    }

    public static GroupWithoutEntities WithoutEntities(this Group self, IMapper mapper)
    {
        var groupDto = mapper.Map<GroupDto>(self);
        return mapper.Map<GroupWithoutEntities>(groupDto);
    }

    public static GroupWithoutEntities WithoutEntities(this GroupDto self, IMapper mapper)
    {
        return mapper.Map<GroupWithoutEntities>(self);
    }
}
