using System.Text;
using System.Text.Json;
using Models;

namespace TechNewsScraper.Output;

public static class ReportWriter
{
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    public static async Task WriteJsonAsync(News report, string path)
{
    var dir = Path.GetDirectoryName(path);
    if (!string.IsNullOrEmpty(dir))
        Directory.CreateDirectory(dir);

    var json = JsonSerializer.Serialize(report, JsonOptions);
    await File.WriteAllTextAsync(path, json, Encoding.UTF8);
    Console.WriteLine($"[Output] JSON salvo em: {path}");
}
    public static async Task WriteMarkdownAsync(News report, string path)
    {
        var sb = new StringBuilder();
        sb.AppendLine("#Tech News Report");
        sb.AppendLine();
        sb.AppendLine($"> Gerado em: {report.DataNoticia:dd/MM/yyyy HH:mm} UTC | Total: {report.NumeroArtigos} artigos");
        sb.AppendLine();

        foreach (var source in report.Artigo.GroupBy(a => a.Fonte))
        {
            sb.AppendLine($"## {source.Key}");
            sb.AppendLine();

            foreach (var article in source.OrderByDescending(a => a.Score))
            {
                sb.AppendLine($"- **[{article.Titulo}]({article.Url})** — {article.Score}");

                if (article.Tag.Count > 0)
                    sb.AppendLine($"  `{string.Join("` `", article.Tag.Take(4))}`");
            }

            sb.AppendLine();
        }

        await File.WriteAllTextAsync(path, sb.ToString(), Encoding.UTF8);
        Console.WriteLine($"[Output] Markdown salvo em: {path}");
    }
}