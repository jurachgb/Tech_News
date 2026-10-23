using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using Microsoft.VisualBasic;
using Models;
using ClassCreator;


namespace TechNewsScraper.Output;

public static class ReportWriter
{
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    public static async Task WriteJsonAsync(News report, string path)
    {
        var reports =new List<News>();
        var hoje=report.DataNoticia.Date;
        if(File.Exists(path))
        {
            var texto =await File.ReadAllTextAsync(path,Encoding.UTF8);
            try
            {
                reports =JsonSerializer.Deserialize<List<News>>(texto)?? [];

            }
            catch(JsonException)
            {
                try
                {
                    var reportAnterior=JsonSerializer.Deserialize<News>(texto);
                    if (reportAnterior is not null)
                    {
                        reports.Add(reportAnterior);
                    }
                    }
                catch(JsonException ex)
                {
                   
                    Console.WriteLine($"[Output] Aviso: não foi possível ler {path} ({ex.Message}). " +
                        "O arquivo existente será preservado em .bak e um novo histórico será iniciado.");
                    var backupPath = $"{path}.bak-{DateTime.UtcNow:yyyyMMddHHmmss}";
                    File.Copy(path, backupPath, overwrite: true);
                }
            }
        }
        var artigosDeHoje = reports
        .Where(r => r.DataNoticia.Date == hoje)
        .SelectMany(r => r.Artigo)
        .Concat(report.Artigo)
        .GroupBy(a => a.Url)
        .Select(g => g.Last())
        .ToList();
        
        foreach(var article in artigosDeHoje)
        {
             await NewsClass.Tag_AI(article);
        }

        var relatoriosAntigos = reports
        .Where(r => r.DataNoticia.Date != hoje)
        .ToList();

    if (artigosDeHoje.Count > 0)
    {
        relatoriosAntigos.Add(new News
        {
            DataNoticia = report.DataNoticia,
            NumeroArtigos = artigosDeHoje.Count,
            Artigo = artigosDeHoje
        });
    }

        reports = relatoriosAntigos;
        var json =JsonSerializer.Serialize(reports ,JsonOptions);
        await File.WriteAllTextAsync(path,json,Encoding.UTF8);
        Console.WriteLine($"[Output] Json salvo em {path}");
    }

    public static async Task WriteMarkdownAsync(News report, string path)
    {
        string historico = "output/NewsHistory";

        if (!Directory.Exists(historico))
        {
            Directory.CreateDirectory(historico);
        }
        if (File.Exists(path))
        {
            string nomeHistorico = Path.Combine(
                historico,
                $"noticia-{report.DataNoticia:dd-MM-yyyy}.md"
            );
            if (!File.Exists(nomeHistorico))
            {
                File.Copy(path, nomeHistorico);
                Console.WriteLine($"[History] Relatório arquivado em: {nomeHistorico}");
            }
        }

        var sb = new StringBuilder();
        sb.AppendLine("# Tech News Report");
        sb.AppendLine();
        sb.AppendLine(
            $"> Gerado em: {report.DataNoticia:dd/MM/yyyy HH:mm} UTC | Total: {report.NumeroArtigos} artigos"
        );

        sb.AppendLine();
        foreach (var source in report.Artigo.GroupBy(a => a.Fonte))
        {
            sb.AppendLine($"## {source.Key}");
            sb.AppendLine();
            foreach (var article in source.OrderByDescending(a => a.Score))
            {
                sb.AppendLine(
                    $"- **[{article.Titulo}]({article.Url})** — Score: {article.Score}"
                );
                if (article.Tag != null && article.Tag.Count > 0)
                {
                    sb.AppendLine(
                        $"  `{string.Join("` `", article.Tag.Take(4))}`"
                    );
                }
                else
                {
                    await NewsClass.Tag_AI(article);
                    if(article.Tag.Count>0)
                    {
                        sb.AppendLine(
                            $"  `{string.Join("` `", article.Tag.Take(4))}`"
                        );
                    }
                }
            }

            sb.AppendLine();
        }
        await File.WriteAllTextAsync(
            path,
            sb.ToString(),
            Encoding.UTF8
        );

        Console.WriteLine($"[Output] Markdown salvo em: {path}");
    }

}
