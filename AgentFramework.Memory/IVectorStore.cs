namespace AgentFramework.Memory;

public interface IVectorStore
{
    Task AddDocumentsAsync(IEnumerable<Document> documents, CancellationToken cancellationToken = default);
    Task<IEnumerable<Document>> SearchAsync(string query, int topK = 3, CancellationToken cancellationToken = default);
}
