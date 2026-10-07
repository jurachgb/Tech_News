namespace Estatisticas;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.Data.Sqlite;
using Database.Models;
using Models;

public static class Statistic
{
    public static string DefaultDbPath = Path.Combine("output", "news.db");

    public static void InitializeDatabase(string dbPath = "")
    {
        if (string.IsNullOrEmpty(dbPath)) dbPath = DefaultDbPath;

        var directory = Path.GetDirectoryName(dbPath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        using var connection = new SqliteConnection($"Data Source={dbPath}");
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            CREATE TABLE IF NOT EXISTS CounTag (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Tag TEXT NOT NULL UNIQUE,
                Count INTEGER NOT NULL
            );
        ";
        command.ExecuteNonQuery();
    }

    public static List<CounTag> CalculateAndSaveTagCounts(List<NewsArticle> articles, string dbPath = "")
    {
        if (string.IsNullOrEmpty(dbPath)) dbPath = DefaultDbPath;
        InitializeDatabase(dbPath);

        var tagCounts = articles
            .SelectMany(a => a.Tag)
            .Where(t => !string.IsNullOrWhiteSpace(t))
            .GroupBy(t => t, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(g => g.Key, g => g.Count());

        using (var connection = new SqliteConnection($"Data Source={dbPath}"))
        {
            connection.Open();
            using var transaction = connection.BeginTransaction();

            var clearCmd = connection.CreateCommand();
            clearCmd.CommandText = "DELETE FROM CounTag;";
            clearCmd.ExecuteNonQuery();

            foreach (var kvp in tagCounts)
            {
                var insertCmd = connection.CreateCommand();
                insertCmd.CommandText = "INSERT INTO CounTag (Tag, Count) VALUES (@Tag, @Count);";
                insertCmd.Parameters.AddWithValue("@Tag", kvp.Key);
                insertCmd.Parameters.AddWithValue("@Count", kvp.Value);
                insertCmd.ExecuteNonQuery();
            }

            transaction.Commit();
        }

        Console.WriteLine($"[Database] {tagCounts.Count} tags salvas no banco SQL ({dbPath}).");
        return GetTagCountsFromDb(dbPath);
    }

    public static List<CounTag> GetTagCountsFromDb(string dbPath = "")
    {
        if (string.IsNullOrEmpty(dbPath)) dbPath = DefaultDbPath;
        InitializeDatabase(dbPath);

        var list = new List<CounTag>();
        using var connection = new SqliteConnection($"Data Source={dbPath}");
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT Id, Tag, Count FROM CounTag ORDER BY Count DESC;";

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            list.Add(new CounTag
            {
                Id = reader.GetInt32(0),
                Tag = reader.GetString(1),
                Count = reader.GetInt32(2)
            });
        }

        return list;
    }

    public static Dictionary<string, int> PrintStatistics(string file)
    {
        if (!File.Exists(file))
        {
            Console.WriteLine($"[Statistics] O arquivo '{file}' não existe.");
            return new Dictionary<string, int>();
        }

        try
        {
            var texto = File.ReadAllText(file).Trim();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var reports = JsonSerializer.Deserialize<List<News>>(texto, options) ?? new List<News>();

            var tags = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            foreach (var report in reports)
            {
                if (report?.Artigo is null) continue;
                foreach (var artigo in report.Artigo)
                {
                    if (artigo?.Tag is null) continue;
                    foreach (var tag in artigo.Tag)
                    {
                        if (string.IsNullOrWhiteSpace(tag)) continue;
                        tags[tag] = tags.GetValueOrDefault(tag, 0) + 1;
                    }
                }
            }

            return tags.OrderByDescending(kv => kv.Value)
                       .ToDictionary(kv => kv.Key, kv => kv.Value);
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"[Statistics] Erro ao ler o arquivo JSON: {ex.Message}");
            return new Dictionary<string, int>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Statistics] Erro: {ex.Message}");
            return new Dictionary<string, int>();
        }
    }

    public static void SaveTagCountsToJson(List<CounTag> tagCounts, string outputPath)
    {
        var options = new System.Text.Json.JsonSerializerOptions { WriteIndented = true };
        var json = System.Text.Json.JsonSerializer.Serialize(tagCounts, options);
        File.WriteAllText(outputPath, json);
    }
}