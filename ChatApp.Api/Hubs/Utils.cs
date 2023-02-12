namespace ChatApp.Api.Hubs;

internal class Utils
{
    internal static async Task<GroupConnection?> RetrieveGroup(
        CacheContext cacheContext,
        int groupId,
        bool strict = false)
    {
        var group = await cacheContext.Groups.FirstOrDefaultAsync(g => g.GroupId == groupId);
        if (group is null)
        {
            if (strict) return null;
            group = new GroupConnection()
            {
                Id = Guid.NewGuid().ToString(),
                GroupId = groupId,
            };
            await cacheContext.Groups.AddAsync(group);
            await cacheContext.SaveChangesAsync();
        }
        return group;
    }
}
