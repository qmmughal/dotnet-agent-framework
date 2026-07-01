using AgentFramework.Core;

namespace AgentFramework.Orchestration;

public interface IWorkflow
{
    IWorkflow AddAgent(IAgent agent);
    Task<IOrchestrationContext> RunAsync(IOrchestrationContext initialContext, CancellationToken cancellationToken = default);
}
