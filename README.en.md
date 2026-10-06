# TechNewsScraper

A technology news scraper built in C# to gather relevant articles from popular sources such as Hacker News and Dev.to, then export them as JSON and Markdown files.

## Features

- Fetches the top stories from Hacker News
- Fetches articles from Dev.to using tags such as .NET, C#, AI, webdev, and DevOps
- Removes duplicate URLs
- Sorts results by score
- Generates reports in:
  - `output/news.json`
  - `output/news.md`

- Automatic tagging of articles using a generative model (Gemini) when tags are missing
- Automatic archiving of Markdown reports to `output/NewsHistory`
- Tag statistics generation utility available in `statistics/statistic.cs`
- Automatic backup of the JSON file (`.bak-<timestamp>`) if the existing file cannot be parsed

## How it works

The project uses `HttpClient` to call public APIs from the supported sources and organizes the data in shared models. After that, the program writes the collected results to files in the `output` folder.

## Requirements

- .NET 10 SDK
- Internet connection

### AI tagging

- To enable automatic tagging, the application calls a generative model (Gemini). Set the environment variable `GEMINI_API_KEY` with your API key before running the program.
- The code includes a short delay between AI requests to avoid hitting rate limits.

##  How to run

From the project directory, execute:

```bash
dotnet run
```

When the process finishes, the files will be created in:

```text
output/
├── news.json
└── news.md
```

## Main structure

- `Program.cs` — application entry point
- `Services/HackerNews.cs` — Hacker News API integration
- `Services/Dev-to.cs` — Dev.to API integration
- `Output/Writer.cs` — JSON and Markdown report generation
- `Models/Artigo.cs` — data models

## Notes

- The app sends a User-Agent for HTTP requests
- Data collection is performed in parallel to improve speed
- Items without a URL are ignored to avoid invalid entries

- Statistics: there is a utility in `statistics/statistic.cs` exposing `Estatisticas.Statistic.PrintStatistics(string file)` which returns a dictionary of tag counts. Example usage in C#:

```csharp
var stats = Estatisticas.Statistic.PrintStatistics("output/news.json");
foreach(var kv in stats) Console.WriteLine($"{kv.Key}: {kv.Value}");
```

## Purpose

This project aims to provide an automated summary of technical news and articles, making it easier to track trends and relevant content from the developer community.
