namespace ChatApp.Api.Data;

public class AppDbContext : IdentityDbContext<AppUser>
{
	// CONSTRUCTOR
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
	{

	}

	// DbSets
	public DbSet<Message> Messages { get; set; } = null!;
	public DbSet<Group> Groups { get; set; } = null!;
	public DbSet<Conversation> Conversations { get; set; } = null!;
}
