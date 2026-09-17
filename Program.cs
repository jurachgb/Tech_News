using Models;
using TechNewsScraper.Output;
using TechNewsScraper.Services;

Console.WriteLine("Tech News Scraper iniciando...\n");


Directory.CreateDirectory("output");

var httpClient = new HttpClient();
httpClient.DefaultRequestHeaders.Add("User-Agent", "TechNewsScraper/1.0");

var hackerNewsService = new HackerNewsService(httpClient);
var devToService = new DevToService(httpClient);

var hnTask = hackerNewsService.GetTopStoriesAsync(limit: 30);
var devToTask = devToService.GetArticlesAsync(perTag: 10);
await Task.WhenAll(hnTask, devToTask);

var allArticles = hnTask.Result.Concat(devToTask.Result).ToList();

var report = new News
{
    DataNoticia = DateTime.UtcNow,
    NumeroArtigos = allArticles.Count,
    Artigo = allArticles
};

await ReportWriter.WriteJsonAsync(report, "output/news.json");
await ReportWriter.WriteMarkdownAsync(report, "output/news.md");

Console.WriteLine($"\n Concluído! {report.NumeroArtigos} artigos coletados.");