using System.Globalization;
using System.Text.Json;

namespace Limpeza;

public static class GetData
{
    public static void GetDataAsyncMd()
    {
        const string path = "output/NewsHistory";
        if (!Directory.Exists(path))
        {
            return;
        }
        var limite = DateTime.UtcNow.Date.AddDays(-30);
        var files = Directory.EnumerateFiles(path, "noticia-*.md");
        foreach (var file in files)
        {
            var fileName = Path.GetFileNameWithoutExtension(file);
            var dateText = fileName["noticia-".Length..];
            if (DateTime.TryParseExact(
                    dateText,
                    "dd-MM-yyyy",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out var fileDate) && fileDate.Date < limite)
            {
                File.Delete(file);
            }
        }
    }

    public static async Task GetDataAsyncJson()
    {
        const string path = "output/news.json";
        if (!File.Exists(path))
        {
            return;
        }
        try
        {
            var json = await File.ReadAllTextAsync(path);
            var reports = JsonSerializer.Deserialize<List<Models.News>>(json);
            if (reports is null)
            {
                return;
            }
            var limite = DateTime.UtcNow.AddDays(-30);
            reports.RemoveAll(report => report.DataNoticia < limite);
            var options = new JsonSerializerOptions { WriteIndented = true };
            var updatedJson = JsonSerializer.Serialize(reports, options);
            await File.WriteAllTextAsync(path, updatedJson);
        }
        catch (JsonException)
        {
            //pula por enquanto
        }
    }
}