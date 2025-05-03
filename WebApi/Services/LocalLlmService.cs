using System.Text;
using System.Text.Json;
using Entities;
using WebApi.ServiceContracts;

namespace WebApi.Services;

public class LocalLlmService : ILlmService
{
    private readonly HttpClient _httpClient;

    public LocalLlmService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetThoughFromPrompt(string title, string body)
    {
        var today = DateTime.Today;
        var requestBody = new
        {
            model = "qwen3-0.6b",
            messages = new[]
            {
                new
                {
                    role = "system",
                    content =
                        "You are analyzing a user message and creating insightful thoughts with meaningful observations and relevant follow-up reminders for a system that save's the user's thoughts."
                },
                new
                {
                    role = "user",
                    content =
                        $"User's thought's title: {title}. User's thought's body: {body}. Taking into account this information, you have to pick to return from only a few options that best fit the user's thought, use the chosen option to fill the JSON template and not output anything else: {{ \"Option\": \"Reminder\" or \"Notification\" or \"Shopping list\" or \"Reflection\"}}."
                }
            }
        };

        var requestContent = new StringContent(
            JsonSerializer.Serialize(requestBody),
            Encoding.UTF8,
            "application/json"
        );

        var response = await _httpClient.PostAsync(
            "http://127.0.0.1:1234/v1/chat/completions", requestContent);

        var responseString = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Response: {responseString}");

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"Request failed with status code {response.StatusCode}");
        }

        var responseJson =
            JsonSerializer.Deserialize<JsonElement>(responseString);
        var content = responseJson
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();

        // Extract and clean the JSON string
        var jsonContent = SanitizeJsonString(content);

        // Create a new thought with default values first
        // var thought = new Thought();

        // Parse JSON with recovery logic for common issues
        using var document = JsonDocument.Parse(jsonContent);
        var root = document.RootElement;

        // Extract Content if present
        if (root.TryGetProperty("Option", out var outputElement) && 
            outputElement.ValueKind != JsonValueKind.Null)
        {
            return outputElement.GetString();
        }

        return "";
    }

    private string SanitizeJsonString(string content)
    {
        // Remove markdown code blocks
        content = content.Replace("```json", "").Replace("```", "");

        // Find the first { and last }
        int start = content.IndexOf('{');
        int end = content.LastIndexOf('}');

        if (start >= 0 && end > start)
        {
            content = content.Substring(start, end - start + 1);
        }

        // Fix common JSON formatting errors
        content =
            content.Replace("\".",
                "\""); // Fix erroneous period after quote
        content = content.Replace("],\"", "],"); // Fix missing spaces
        content = content.Replace("}\",", "\"},"); // Fix quote positions

        // Fix missing commas between array items
        content = System.Text.RegularExpressions.Regex.Replace(
            content,
            "\"\\s*\"",
            "\",\"");

        return content;
    }

    private string ExtractValueBetweenQuotes(string json,
        string propertyName)
    {
        var pattern = $"\"{propertyName}\"\\s*:\\s*\"([^\"]*)\"";
        var match =
            System.Text.RegularExpressions.Regex.Match(json, pattern);
        return match.Success ? match.Groups[1].Value : string.Empty;
    }
}