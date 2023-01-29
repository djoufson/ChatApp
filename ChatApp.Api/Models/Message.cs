using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApp.Api.Models;

public class Message
{
    [Key] 
    public int Id { get; set; }
    
    [ForeignKey(nameof(AppUser))] 
    public string FromUserId { get; set; } = null!;
    [ForeignKey(nameof(AppUser))]
    public string ToUserId { get; set; } = null!;
    public AppUser FromUser { get; set; } = null!;
    public string Content { get; set; } = null!;

    [ForeignKey(nameof(Group))] 
    public int? GroupId { get; set; } // The id of the related group if exists
    public Group? Group { get; set; } // The related group if exists
}
