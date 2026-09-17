# TechNewsScraper

A technology news scraper built in C# to gather relevant articles from popular sources such as Hacker News and Dev.to, then export them as JSON and Markdown files.

## ✨ Features

- Fetches the top stories from Hacker News
- Fetches articles from Dev.to using tags such as .NET, C#, AI, webdev, and DevOps
- Removes duplicate URLs
- Sorts results by score
- Generates reports in:
  - `output/news.json`
  - `output/news.md`

## 🧩 How it works

The project uses `HttpClient` to call public APIs from the supported sources and organizes the data in shared models. After that, the program writes the collected results to files in the `output` folder.

## ✅ Requirements

- .NET 10 SDK
- Internet connection

## ▶️ How to run

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

## 📁 Main structure

- `Program.cs` — application entry point
- `Services/HackerNews.cs` — Hacker News API integration
- `Services/Dev-to.cs` — Dev.to API integration
- `Output/Writer.cs` — JSON and Markdown report generation
- `Models/Artigo.cs` — data models

## 🔎 Notes

- The app sends a User-Agent for HTTP requests
- Data collection is performed in parallel to improve speed
- Items without a URL are ignored to avoid invalid entries

## 📌 Purpose

This project aims to provide an automated summary of technical news and articles, making it easier to track trends and relevant content from the developer community.
