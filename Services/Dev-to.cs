using System.Text.Json;
using Models;

namespace TechNewsScraper.Services;

public class DevToService(HttpClient httpClient)
{
    private const string BaseUrl = "https://dev.to/api/articles";
    private static readonly string[] Tags = ["dotnet", "csharp", "ai", "webdev", "devops", "opensource"];

    public async Task<List<NewsArticle>> GetArticlesAsync(int perTag = 10)
    {
        Console.WriteLine("[Dev.to] Buscando artigos por tags...");

        var articles = new List<NewsArticle>();
        var seen = new HashSet<string>(); // evita duplicatas

        foreach (var tag in Tags)
        {
            try
            {
                var json = await httpClient.GetStringAsync($"{BaseUrl}?tag={tag}&per_page={perTag}&top=1");
                using var doc = JsonDocument.Parse(json);

                foreach (var item in doc.RootElement.EnumerateArray())
                {
                    var url = item.TryGetProperty("url", out var u) ? u.GetString() ?? "" : "";

                    // Pula se não tem URL ou já foi adicionado
                    if (string.IsNullOrEmpty(url) || !seen.Add(url)) continue;

                    var tagList = new List<string>();
                    if (item.TryGetProperty("tag_list", out var tl))
                        foreach (var t in tl.EnumerateArray())
                            tagList.Add(t.GetString() ?? "");

                    articles.Add(new NewsArticle
                    {
                        Titulo  = item.TryGetProperty("title", out var title) ? title.GetString() ?? "" : "",
                        Url    = url,
                        Fonte = "Dev.to",
                        Score  = item.TryGetProperty("positive_reactions_count", out var r) ? r.GetInt32() : 0,
                        Tag   = tagList
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Dev.to] Erro na tag '{tag}': {ex.Message}");
            }
        }

        Console.WriteLine($"[Dev.to] {articles.Count} artigos coletados.");
        return articles;
    }
}