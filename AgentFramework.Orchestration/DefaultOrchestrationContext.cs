using System.Collections.Concurrent;
using AgentFramework.Core;

namespace AgentFramework.Orchestration;

public class DefaultOrchestrationContext : IOrchestrationContext
{
    private readonly ConcurrentDictionary<string, object> _state = new();
    private readonly List<ChatMessage> _conversationHistory = new();
    private readonly object _lock = new();

    public IReadOnlyDictionary<string, object> State => _state;
    public IReadOnlyList<ChatMessage> ConversationHistory
    {
        get
        {
            lock (_lock)
            {
                return _conversationHistory.ToList();
            }
        }
    }

    public void AddMessage(ChatMessage message)
    {
        lock (_lock)
        {
            _conversationHistory.Add(message);
        }
    }

    public T? GetState<T>(string key)
    {
        if (_state.TryGetValue(key, out var value) && value is T typedValue)
        {
            return typedValue;
        }
        return default;
    }

    public void SetState<T>(string key, T value)
    {
        if (value != null)
        {
            _state[key] = value;
        }
    }
}
