using System.Collections.Concurrent;

namespace AgentFramework.Memory;

/// <summary>
/// A simple in-memory vector store that simulates semantic search using basic text matching for demonstration purposes.
/// </summary>
public class InMemoryVectorStore : IVectorStore
{
    private readonly ConcurrentBag<Document> _documents = new();

    public Task AddDocumentsAsync(IEnumerable<Document> documents, CancellationToken cancellationToken = default)
    {
        foreach (var doc in documents)
        {
            _documents.Add(doc);
        }
        return Task.CompletedTask;
    }

    public Task<IEnumerable<Document>> SearchAsync(string query, int topK = 3, CancellationToken cancellationToken = default)
    {
        var queryTerms = query.ToLowerInvariant().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        
        var results = _documents
            .Select(d => new 
            { 
                Doc = d, 
                Score = queryTerms.Count(term => d.Content.ToLowerInvariant().Contains(term)) 
            })
            .Where(x => x.Score > 0)
            .OrderByDescending(x => x.Score)
            .Take(topK)
            .Select(x => x.Doc)
            .ToList();

        return Task.FromResult<IEnumerable<Document>>(results);
    }
}
