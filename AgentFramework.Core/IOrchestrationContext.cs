namespace AgentFramework.Core;

public interface IOrchestrationContext
{
    IReadOnlyDictionary<string, object> State { get; }
    IReadOnlyList<ChatMessage> ConversationHistory { get; }

    void AddMessage(ChatMessage message);
    T? GetState<T>(string key);
    void SetState<T>(string key, T value);
}
