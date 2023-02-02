using ChatApp.Console.Models;

namespace ChatApp.Console.Repository;

public class Database
{
    public List<User> Users { get; private set; }
    public List<Message> Messages { get; private set; }
    public List<Conversation> Conversations { get; private set; }
    public static Database Instance { get; } = new Database();
    private Database()
    {
        Users = new ();
        Messages = new ();
        Conversations = new ();
    }

    internal void Seed()
    {
        Users = new List<User>()
        {
            new User()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Djoufson",
                Conversations = new()
            },
            new User()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "John Doe",
                Conversations = new()
            },
            new User()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Jane Smith",
                Conversations = new()
            }
        };

        Conversations = new()
        {
            new Conversation()
            {
                Id = 1,
                User1Id = Users[0].Id,
                User2Id = Users[1].Id,
                Messages = new()
            },
            new Conversation()
            {
                Id = 2,
                User1Id = Users[0].Id,
                User2Id = Users[2].Id,
                Messages = new()
            },
            new Conversation()
            {
                Id = 3,
                User1Id = Users[1].Id,
                User2Id = Users[2].Id,
                Messages = new()
            },
        };

        Messages = new()
        {
            new Message()
            {
                Id = 1,
                ConversationId = 1,
                FromUserId = Conversations.FirstOrDefault(c => c.Id == 1)?.User1Id!,
                ToUserId = Conversations.FirstOrDefault(c => c.Id == 1)?.User2Id!,
                Content = "Hello!"
            },
            new Message()
            {
                Id = 2,
                ConversationId = 1,
                FromUserId = Conversations.FirstOrDefault(c => c.Id == 1)?.User2Id!,
                ToUserId = Conversations.FirstOrDefault(c => c.Id == 1)?.User1Id!,
                Content = "Yess"
            },
            new Message()
            {
                Id = 3,
                ConversationId = 1,
                FromUserId = Conversations.FirstOrDefault(c => c.Id == 1)?.User1Id!,
                ToUserId = Conversations.FirstOrDefault(c => c.Id == 1)?.User2Id!,
                Content = "How are you?"
            },
        };
    }
}
