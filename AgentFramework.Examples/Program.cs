using AgentFramework.Core;
using AgentFramework.Memory;
using AgentFramework.Orchestration;
using AgentFramework.Providers.OpenAI;
using AgentFramework.Examples;

Console.WriteLine("=== .NET Multi-Agent Orchestration Framework Example ===");

// 1. Setup Vector Store & Tools
var vectorStore = new InMemoryVectorStore();
await vectorStore.AddDocumentsAsync(new[]
{
    new Document { Content = "LangChain is a framework for developing applications powered by language models." },
    new Document { Content = "Semantic Kernel is an SDK that integrates Large Language Models (LLMs) with conventional programming languages." },
    new Document { Content = "CrewAI is a framework for orchestrating role-playing autonomous AI agents." }
});

var searchTool = new VectorSearchTool(vectorStore);

// 2. Setup LLM Provider
// To use real OpenAI: var llmProvider = new OpenAILlmProvider("sk-your-api-key");
var llmProvider = new DummyLlmProvider();

// 3. Define Agents
var researcherAgent = new Agent(
    name: "Research Agent",
    role: "Senior AI Researcher",
    goal: "Gather information about AI orchestration frameworks.",
    backstory: "You are an expert in AI frameworks and provide summaries for developers.",
    llmProvider: llmProvider,
    tools: new[] { searchTool }
);

var developerAgent = new Agent(
    name: "Developer Agent",
    role: "Senior .NET Developer",
    goal: "Write clean, idiomatic C# code based on research.",
    backstory: "You specialize in .NET 8/9 and write high-performance APIs.",
    llmProvider: llmProvider
);

var reviewerAgent = new Agent(
    name: "Reviewer Agent",
    role: "Principal Software Engineer",
    goal: "Review code and provide final approval.",
    backstory: "You ensure code meets enterprise standards before it is shipped.",
    llmProvider: llmProvider
);

// 4. Create and Run Workflow
var workflow = new SequentialWorkflow()
    .AddAgent(researcherAgent)
    .AddAgent(developerAgent)
    .AddAgent(reviewerAgent);

Console.WriteLine("Starting workflow...\n");

var context = new DefaultOrchestrationContext();
context.AddMessage(new ChatMessage(ChatRole.User, "Please build a small C# class that represents an AI agent, using the research on existing frameworks."));

var finalContext = await workflow.RunAsync(context);

Console.WriteLine("\n=== Workflow Completed ===");
Console.WriteLine("Conversation History:\n");

foreach (var msg in finalContext.ConversationHistory)
{
    Console.WriteLine($"[{msg.Role}] {msg.Content}\n");
}
