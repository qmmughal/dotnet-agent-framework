using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using AgentFramework.Core;

namespace AgentFramework.Providers.OpenAI;

public class OpenAILlmProvider : ILlmProvider
{
    private readonly HttpClient _httpClient;
    private readonly string _model;

    public OpenAILlmProvider(string apiKey, string model = "gpt-4o", HttpClient? httpClient = null)
    {
        _model = model;
        _httpClient = httpClient ?? new HttpClient();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        
        if (_httpClient.BaseAddress == null)
        {
            _httpClient.BaseAddress = new Uri("https://api.openai.com/v1/");
        }
    }

    public async Task<LlmResponse> GenerateResponseAsync(LlmRequest request, CancellationToken cancellationToken = default)
    {
        var messages = new List<object>();

        if (!string.IsNullOrWhiteSpace(request.SystemPrompt))
        {
            messages.Add(new { role = "system", content = request.SystemPrompt });
        }

        foreach (var msg in request.Messages)
        {
            messages.Add(new
            {
                role = msg.Role.ToString().ToLowerInvariant(),
                content = msg.Content
            });
        }

        var payload = new
        {
            model = _model,
            messages = messages
        };

        var response = await _httpClient.PostAsJsonAsync("chat/completions", payload, cancellationToken);
        response.EnsureSuccessStatusCode();

        var responseData = await response.Content.ReadFromJsonAsync<JsonElement>(cancellationToken: cancellationToken);
        var content = responseData.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString() ?? string.Empty;

        return new LlmResponse
        {
            Content = content
        };
    }
}
