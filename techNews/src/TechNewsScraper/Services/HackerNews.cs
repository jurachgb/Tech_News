using System.Text.Json;
using Models;

namespace TechNewsScraper.Services;

public class HackerNewsService(HttpClient httpClient)
{
    private const string BaseUrl = "https://hacker-news.firebaseio.com/v0";

    public async Task<List<NewsArticle>> GetTopStoriesAsync(int limit = 30)
    {
        Console.WriteLine("[Hacker News] Buscando top stories...");

        // 1. Busca a lista de IDs
        var idsJson = await httpClient.GetStringAsync($"{BaseUrl}/topstories.json");
        var ids = JsonSerializer.Deserialize<List<int>>(idsJson) ?? [];

        // 2. Busca cada artigo em paralelo
        var tasks = ids.Take(limit).Select(id => FetchItemAsync(id));
        var results = await Task.WhenAll(tasks);

        var articles = results.Where(a => a is not null).Cast<NewsArticle>().ToList();

        Console.WriteLine($"[Hacker News] {articles.Count} artigos coletados.");
        return articles;
    }

    private async Task<NewsArticle?> FetchItemAsync(int id)
    {
        try
        {
            var json = await httpClient.GetStringAsync($"{BaseUrl}/item/{id}.json");
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            // Ignora itens sem URL (ex: posts de discussão)
            if (!root.TryGetProperty("url", out var urlProp)) return null;

            return new NewsArticle
            {
                Titulo = root.TryGetProperty("title", out var t) ? t.GetString() ?? "" : "",
                Url   = urlProp.GetString() ?? "",
                Fonte = "Hacker News",
                Score = root.TryGetProperty("score", out var s) ? s.GetInt32() : 0,
            };
        }
        catch
        {
            return null;
        }
    }
}