namespace AgentFramework.Core;

public class AgentResult
{
    public string Output { get; set; } = string.Empty;
    public bool Success { get; set; }
    public Exception? Error { get; set; }
}
