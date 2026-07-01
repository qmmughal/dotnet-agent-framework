namespace AgentFramework.Core;

public class LlmRequest
{
    public IEnumerable<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
    public IEnumerable<ITool> Tools { get; set; } = new List<ITool>();
    public string SystemPrompt { get; set; } = string.Empty;
}

public class LlmResponse
{
    public string Content { get; set; } = string.Empty;
    public string? ToolCallId { get; set; }
    public string? ToolCallName { get; set; }
    public string? ToolCallArguments { get; set; }
}

public interface ILlmProvider
{
    Task<LlmResponse> GenerateResponseAsync(LlmRequest request, CancellationToken cancellationToken = default);
}
