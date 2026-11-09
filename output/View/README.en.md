# Visualizations and Statistics (output/View)

This folder contains the files generated for the persistence and statistical visualization of articles collected by **TechNewsScraper**.

## Folder Contents

- **`news.db`**: SQLite database containing the `CounTag` table. The application tallies the tags associated with all articles gathered in the current run and stores each tag's frequency (`Id`, `Tag`, `Count`).
- **`chart.html` (or `.html`)**: Standalone HTML page generated with [Chart.js](https://www.chartjs.org/) that renders an interactive pie chart displaying the top 15 most frequent tags from the collected articles.
- **`README.md`**: Documentation in Portuguese explaining the purpose and workflow of this directory.
- **`README.en.md`**: English version of this documentation.

## How the Process Works

1. **Aggregation and Counting (`Estatisticas.Statistic`)**:
   - The collected articles (from Hacker News and Dev.to) are processed to extract and group all tags.
   - The SQLite database `news.db` is initialized (creating the `CounTag` table if it does not exist).
   - Previous records are cleared and populated with the counts from the current run.

2. **Visual Rendering (`Graphics.HtmlCharts`)**:
   - The tag counts are queried from `news.db` ordered in descending order.
   - The top 15 most popular tags are formatted and embedded into an HTML file featuring a responsive pie chart powered by Chart.js (loaded via CDN).

## How to View

To view the graphical statistics, open the generated HTML file (e.g. `chart.html` or `.html`) in any modern web browser with internet access (required to load the Chart.js CDN).
