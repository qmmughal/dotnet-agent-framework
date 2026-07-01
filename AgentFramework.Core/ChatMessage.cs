namespace AgentFramework.Core;

public enum ChatRole
{
    System,
    User,
    Assistant,
    Tool
}

public class ChatMessage
{
    public ChatRole Role { get; }
    public string Content { get; }
    public string? ToolCallId { get; }

    public ChatMessage(ChatRole role, string content, string? toolCallId = null)
    {
        Role = role;
        Content = content;
        ToolCallId = toolCallId;
    }
}
