namespace ClassCreator;

using System.Net.Http.Json;
using System.Text.Json;
using Models;

public static class NewsClass
{
    public static async Task Tag_AI(Models.NewsArticle article)
    {
        if (article.Tag.Count > 0) return;
        await Task.Delay(4000);
        List<string> tagsoption = new List<string>() {
        "ai", "c#","dotnet","c++", "javascript", "python", "java", "go", "rust", "typescript", "kotlin", "swift", "php", "devops", "security", "architecture", "mobile", "cloud", "games",
        "opensource","linux","backend","database","frontend","debugging","performance","debug","microsoft","tutorial","critical","robotics"
        }
        ;
        var apiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY");
        Console.WriteLine($"[Gemini] Key found: {!string.IsNullOrEmpty(apiKey)}");
        
        var client = new HttpClient();
        var request = new
        {
            contents = new[]
            {
            new
            {
                parts=new[]
                {
                    new {text = $"You are a tagging system. Respond ONLY with 1 to 4 tags separated by commas, nothing else. No explanation, no sentences, just the tags. " +
                                $"Available tags: {string.Join(", ", tagsoption)}. If no tag matches use 'Another'. Article title: {article.Titulo}"
                    }
                }
            }
        }

        };
        var response = await client.PostAsJsonAsync($"https://generativelanguage.googleapis.com/v1beta/models/gemini-3.5-flash-lite:generateContent?key={apiKey}", request);

        var result = await response.Content.ReadFromJsonAsync<JsonElement>();
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"[Gemini] API error {response.StatusCode}: {result}");
            return;
        }
        if (!result.TryGetProperty("candidates", out var candidates))
        {
            Console.WriteLine($"[Gemini] Unexpected response: {result}");
            return;
        }

        var text = result
            .GetProperty("candidates")[0]
            .GetProperty("content")
            .GetProperty("parts")[0]
            .GetProperty("text")
            .GetString();

        if (text != null)
        {
            var tags = text.Split(',').Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

            foreach (var tag in tags)
            {
                if (!article.Tag.Contains(tag))
                {
                    article.Tag.Add(tag);
                }
            }
        }
    }
}