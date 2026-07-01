using AgentFramework.Core;

namespace AgentFramework.Orchestration;

public class SequentialWorkflow : IWorkflow
{
    private readonly List<IAgent> _agents = new();

    public IWorkflow AddAgent(IAgent agent)
    {
        _agents.Add(agent);
        return this;
    }

    public async Task<IOrchestrationContext> RunAsync(IOrchestrationContext initialContext, CancellationToken cancellationToken = default)
    {
        var context = initialContext ?? new DefaultOrchestrationContext();

        foreach (var agent in _agents)
        {
            context.AddMessage(new ChatMessage(ChatRole.System, $"--- Transitioning to Agent: {agent.Name} ---"));

            var result = await agent.ExecuteAsync(context, cancellationToken);
            
            if (!result.Success)
            {
                context.AddMessage(new ChatMessage(ChatRole.System, $"Agent {agent.Name} failed: {result.Error?.Message}"));
                break;
            }

            if (!string.IsNullOrWhiteSpace(result.Output))
            {
                 context.AddMessage(new ChatMessage(ChatRole.Assistant, result.Output));
            }
        }

        return context;
    }
}
