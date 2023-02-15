namespace ChatApp.Api.Data;

public class CacheContext : DbContext
{
	public CacheContext(DbContextOptions<CacheContext> options) : base(options)
	{ }
	public DbSet<ChatUserConnection> Connections { get; set; } = null!;
    public DbSet<GroupConnection> Groups { get; set; } = null!;
}
