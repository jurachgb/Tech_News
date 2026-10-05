namespace Estatisticas;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

public static class Statistic
{
    public static Dictionary<string, int> PrintStatistics(string file)
    {
        if (!File.Exists(file))
        {
            Console.WriteLine($"[Statistics] O arquivo '{file}' não existe.");
            return new Dictionary<string, int>();
        }

        var tags = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        string texto = File.ReadAllText(file).Trim();

        using (JsonDocument document = JsonDocument.Parse(texto))
        {
            JsonElement raiz = document.RootElement;
            
            foreach (JsonElement grupo in raiz.EnumerateArray())
            {
                if (grupo.TryGetProperty("Artigo", out JsonElement listaArtigos) && listaArtigos.ValueKind == JsonValueKind.Array)
                {
                    foreach (JsonElement artigo in listaArtigos.EnumerateArray())
                    { 
                        if (artigo.TryGetProperty("Tag", out JsonElement tagsElement) && tagsElement.ValueKind == JsonValueKind.Array)
                        {
                            foreach (JsonElement tag in tagsElement.EnumerateArray())
                            {
                                string tagValue = tag.GetString() ?? "Unknown";
                                tags[tagValue] = tags.GetValueOrDefault(tagValue, 0) + 1;
                            }
                        }
                    }
                }
            }
        }
        return tags.OrderByDescending(kv => kv.Value)
                   .ToDictionary(kv => kv.Key, kv => kv.Value);
    }
}