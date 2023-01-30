namespace ChatApp.Api.Utilities.Extentions;

public static class UserExtensions
{
    public static UserWithoutEntities WithoutEntities(this AppUser self, IMapper mapper)
    {
        var userDto = mapper.Map<UserDto>(self);
        return mapper.Map<UserWithoutEntities>(userDto);
    }

    public static UserWithoutEntities WithoutEntities(this UserDto self, IMapper mapper)
    {
        return mapper.Map<UserWithoutEntities>(self);
    }

    public static UserDto AsDto(this AppUser self, IMapper mapper)
    {
        return mapper.Map<UserDto>(self);
    }
}
