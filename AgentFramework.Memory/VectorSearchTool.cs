using AgentFramework.Core;

namespace AgentFramework.Memory;

public class VectorSearchTool : ITool
{
    private readonly IVectorStore _vectorStore;

    public string Name => "SearchKnowledgeBase";
    public string Description => "Searches the local knowledge base for relevant context or documents.";

    public VectorSearchTool(IVectorStore vectorStore)
    {
        _vectorStore = vectorStore;
    }

    public async Task<string> ExecuteAsync(string input, CancellationToken cancellationToken = default)
    {
        var results = await _vectorStore.SearchAsync(input, topK: 3, cancellationToken);
        var docs = results.ToList();

        if (docs.Count == 0)
        {
            return "No relevant documents found.";
        }

        var combinedContent = string.Join("\n\n---\n\n", docs.Select(d => d.Content));
        return combinedContent;
    }
}
