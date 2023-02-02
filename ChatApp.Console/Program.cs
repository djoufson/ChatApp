using ChatApp.Console.Repository;
using static System.Console;

namespace ChatApp.Console;

public class Program
{
    public static void Main()
    {
        var db = Database.Instance;
        db.Seed();
        //foreach (var conversation in db.Conversations)
        //{
        //    WriteLine(conversation.Id);
        //}
        WriteLine("Les conversations de L'utilisateur 3");
        var userId = db.Users[2].Id;
        var conversations = db.Conversations
            .Where(c => c.User1Id == userId)
            .ToList();

        conversations.AddRange(
                db.Conversations
                .Where(c => c.User2Id == userId)
                .ToList()
            );
        

        foreach (var conversation in conversations)
        {
            WriteLine(conversation.Id);
            //var messages = db.Messages.Where(m => m.ConversationId == conversation.Id);
            //foreach (var message in messages)
            //{
            //    var FromUserName = db.Users
            //        .Where(u => u.Id == message.FromUserId)
            //        .Select(c => c.Name)
            //        .FirstOrDefault();

            //    var ToUserName = db.Users
            //        .Where(u => u.Id == message.ToUserId)
            //        .Select(c => c.Name)
            //        .FirstOrDefault();

            //    WriteLine($"From {FromUserName} To {ToUserName} : \n\t{message.Content}\n");
            //}
        }
    }
}
