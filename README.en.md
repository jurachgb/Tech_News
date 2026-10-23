# TechNewsScraper

A technology news and article scraper built with C#/.NET. The application queries Hacker News and Dev.to, removes duplicate articles, and generates local JSON, Markdown, and HTML reports.

## Features

- Fetches the top 30 stories from Hacker News.
- Fetches Dev.to articles using the `dotnet`, `csharp`, `ai`, `webdev`, `devops`, and `opensource` tags.
- Runs the source queries in parallel.
- Ignores items without a URL and removes duplicate URLs.
- Sorts articles by score in the reports.
- Automatically generates tags with Gemini when an article has no tags.
- Keeps a daily archive of Markdown reports.
- Stores tag counts in a SQLite database.
- Generates an HTML chart with the 15 most-used tags.
- Keeps only the last 30 days in the JSON history and Markdown archive.
- Creates a `.bak-<timestamp>` backup when the existing JSON file cannot be parsed.

## Requirements

- .NET 10 SDK
- Internet access to query the Hacker News and Dev.to APIs
- A Gemini API key if automatic tag generation is desired

## AI configuration

Set `GEMINI_API_KEY` in the environment before running the application:

### PowerShell

```powershell
$env:GEMINI_API_KEY = "your-gemini-api-key"
```

### Windows (persistent for the user)

```powershell
[Environment]::SetEnvironmentVariable("GEMINI_API_KEY", "your-gemini-api-key", "User")
```

If the variable is not defined, the application still collects articles, but Gemini tag-generation requests cannot be completed.

## How to run

Run the commands from the directory containing `TechNewsScraper.csproj`:

```powershell
dotnet restore
dotnet run
```

To run an optimized build:

```powershell
dotnet run --configuration Release
```

When the process finishes, it reports the number of collected articles in the terminal and saves the files in the `output/` folder.

## Generated files

```text
output/
├── news.json                 # structured history for the last 30 days
├── news.md                   # report from the current run
├── NewsHistory/              # dated Markdown reports
└── View/
    ├── news.db               # SQLite database with tag counts
    └── .html                 # chart of the 15 most-used tags
```

The `output/View/.html` file can be opened directly in a browser. The chart uses Chart.js through a CDN and therefore requires internet access to load the library.

## Main structure

```text
Program.cs                         # application entry point and execution flow
Services/HackerNews.cs             # Hacker News API integration
Services/Dev-to.cs                 # Dev.to API integration
Models/                            # report and article models
Writer/Writer.cs                   # JSON and Markdown report writing
statistics/statistic.cs            # statistics persistence and reading
statistics/HtmlCharts.cs           # HTML chart generation
Clear.cs                           # cleanup of data older than 30 days
```

## Statistics

Tag counts are saved to `output/View/news.db` during execution. You can also obtain accumulated counts from a JSON file using `Statistic.PrintStatistics`:

```csharp
var stats = Estatisticas.Statistic.PrintStatistics("output/news.json");

foreach (var (tag, count) in stats)
{
    Console.WriteLine($"{tag}: {count}");
}
```

## Data sources

- [Hacker News API](https://github.com/HackerNews/API)
- [Dev.to API](https://developers.forem.com/api)
- [Google Gemini API](https://ai.google.dev/gemini-api/docs)

## Purpose

Make it easier to follow daily technical news and articles by collecting relevant community content into searchable reports and a visual summary of the most-used tags.
