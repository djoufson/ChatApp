﻿namespace ChatApp.Api.Dtos;

public class MessageWithoutEntities
{
    public int Id { get; set; }
    public string FromUserId { get; set; } = null!;
    public string? ToUserId { get; set; }
    public string Content { get; set; } = null!;
    public int? ConversationId { get; set; }
    public int? GroupId { get; set; }
    public DateTime SentAt { get; set; }
}
