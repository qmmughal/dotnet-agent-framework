using AgentFramework.Core;

namespace AgentFramework.Orchestration;

public class Agent : IAgent
{
    private readonly ILlmProvider _llmProvider;

    public string Name { get; }
    public string Role { get; }
    public string Goal { get; }
    public string Backstory { get; }
    public IReadOnlyCollection<ITool> Tools { get; }

    public Agent(string name, string role, string goal, string backstory, ILlmProvider llmProvider, IEnumerable<ITool>? tools = null)
    {
        Name = name;
        Role = role;
        Goal = goal;
        Backstory = backstory;
        _llmProvider = llmProvider;
        Tools = tools?.ToList() ?? new List<ITool>();
    }

    public async Task<AgentResult> ExecuteAsync(IOrchestrationContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var systemPrompt = $"You are {Name}.\nRole: {Role}\nGoal: {Goal}\nBackstory: {Backstory}";

            var request = new LlmRequest
            {
                SystemPrompt = systemPrompt,
                Messages = context.ConversationHistory,
                Tools = Tools
            };

            var response = await _llmProvider.GenerateResponseAsync(request, cancellationToken);

            return new AgentResult
            {
                Success = true,
                Output = response.Content
            };
        }
        catch (Exception ex)
        {
            return new AgentResult
            {
                Success = false,
                Error = ex
            };
        }
    }
}
