namespace AgentFramework.Core;

public interface ITool
{
    string Name { get; }
    string Description { get; }
    Task<string> ExecuteAsync(string input, CancellationToken cancellationToken = default);
}
