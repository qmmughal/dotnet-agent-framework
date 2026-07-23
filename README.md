# .NET Multi-Agent Orchestration Framework

![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)
![.NET 8](https://img.shields.io/badge/.NET-8.0-purple.svg)

A lightweight, enterprise-ready multi-agent orchestration framework designed for the .NET ecosystem. 

Python-centric tools like CrewAI or LangChain are great, but many enterprise systems run on .NET. This framework fills that gap by providing native, idiomatic C# abstractions for building autonomous agents, equipping them with tools, and orchestrating complex workflows.

## Features

- **Native .NET**: Built targeting .NET 8/9 with minimal external dependencies.
- **Agent Abstraction**: Define agents with distinct personas (Role, Goal, Backstory) and give them specific tools to accomplish their objectives.
- **Extensible Orchestration**: Chain agents together via `SequentialWorkflow` to pass conversational context, memory, and state smoothly from one agent to the next.
- **LLM Agnostic**: The `ILlmProvider` interface makes it trivial to swap backends (OpenAI, Semantic Kernel, local Ollama) by simply changing the provider injection.
- **RAG Ready**: Built-in simple `IVectorStore` interfaces with an `InMemoryVectorStore` example to get you started with Retrieval-Augmented Generation immediately.

## Getting Started

### 1. Define your LLM Provider
Implement `ILlmProvider` or use the included `OpenAILlmProvider`.
```csharp
var llmProvider = new OpenAILlmProvider("sk-your-openai-api-key");
```

### 2. Equip Agents with Tools
Tools implement the `ITool` interface. Give your agents access to search, APIs, or databases.
```csharp
var vectorStore = new InMemoryVectorStore(); // Load with data
var searchTool = new VectorSearchTool(vectorStore);
```

### 3. Create Agents
Assign roles and tools to your agents.
```csharp
var researcherAgent = new Agent(
    name: "Research Agent",
    role: "Senior AI Researcher",
    goal: "Gather information about AI orchestration frameworks.",
    backstory: "You are an expert in AI frameworks and provide summaries for developers.",
    llmProvider: llmProvider,
    tools: new[] { searchTool }
);
```

### 4. Orchestrate
Link agents into a workflow.
```csharp
var workflow = new SequentialWorkflow()
    .AddAgent(researcherAgent)
    .AddAgent(developerAgent)
    .AddAgent(reviewerAgent);

var context = new DefaultOrchestrationContext();
context.AddMessage(new ChatMessage(ChatRole.User, "Build an Agent class."));

var finalContext = await workflow.RunAsync(context);
```

## Publishing

All four library projects (`AgentFramework.Core`, `.Memory`, `.Orchestration`, `.Providers.OpenAI`) carry full NuGet metadata and are packed/pushed as a set by `.github/workflows/publish-nuget.yml` whenever a `v*` tag is pushed:

```bash
git tag v0.1.0
git push origin v0.1.0
```

Requires a `NUGET_API_KEY` repo secret with push rights on nuget.org. `AgentFramework.Examples` is intentionally excluded — it's a console demo, not a library.

## Contributing

We welcome contributions! Please see our [Contributing Guide](CONTRIBUTING.md) for more details.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

*Machine-readable repository context for AI agents and crawlers lives in [`llms.txt`](llms.txt).*
