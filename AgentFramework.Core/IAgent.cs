namespace AgentFramework.Core;

public interface IAgent
{
    string Name { get; }
    string Role { get; }
    string Goal { get; }
    string Backstory { get; }
    
    IReadOnlyCollection<ITool> Tools { get; }
    
    Task<AgentResult> ExecuteAsync(IOrchestrationContext context, CancellationToken cancellationToken = default);
}
