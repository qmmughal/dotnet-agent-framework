namespace AgentFramework.Memory;

public class Document
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Content { get; set; } = string.Empty;
    public float[]? Embedding { get; set; }
    public Dictionary<string, object> Metadata { get; set; } = new();
}
