using AgentFramework.Core;

namespace AgentFramework.Examples;

public class DummyLlmProvider : ILlmProvider
{
    public Task<LlmResponse> GenerateResponseAsync(LlmRequest request, CancellationToken cancellationToken = default)
    {
        var role = request.SystemPrompt.Contains("Researcher") ? "Researcher" :
                   request.SystemPrompt.Contains("Developer") ? "Developer" : "Reviewer";

        string response = role switch
        {
            "Researcher" => "I searched the knowledge base. Here are the frameworks I found: LangChain, Semantic Kernel, and CrewAI.",
            "Developer" => "Based on the research, here is the basic Agent class:\n```csharp\npublic class Agent { public string Name { get; set; } }\n```",
            "Reviewer" => "The implementation looks clean. Approved.",
            _ => "Generic response."
        };

        // Simulate network delay
        Thread.Sleep(500);

        return Task.FromResult(new LlmResponse { Content = response });
    }
}
