namespace Graphics;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Database.Models;
using Estatisticas;

public static class HtmlCharts
{
    public static void GenerateHtmlChartFromDb(string dbPath = "", string outputPath = "output/View/chart.html")
    {
        var tagCounts = Statistic.GetTagCountsFromDb(dbPath);
        var tagDict = tagCounts.ToDictionary(t => t.Tag, t => t.Count);
        GenerateHtmlChart(tagDict, outputPath);
    }

    public static void GenerateHtmlChart(Dictionary<string, int> tags, string outputPath)
    {
        var top = tags.OrderByDescending(t => t.Value).Take(15).ToList();

        var labels = string.Join(",", top.Select(t => $"\"{t.Key}\""));
        var values = string.Join(",", top.Select(t => t.Value));

        string html = $$"""
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset="UTF-8">
                <title>Tech News Statistics</title>
                <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
            </head>
            <body>
                <h1>Most Used Tags (Last 30 Days)</h1>
                <canvas id="chart" width="800" height="800"></canvas>
                <script>
                    new Chart(document.getElementById('chart'), {
                        type: 'pie',
                        data: {
                            labels: [{{labels}}],
                            datasets: [{
                                data: [{{values}}]
                            }]
                        }
                    });
                </script>
            </body>
            </html>
            """;

        var dir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        File.WriteAllText(outputPath, html, Encoding.UTF8);
        Console.WriteLine($"[Statistics] HTML gerado em {outputPath}");
    }
}