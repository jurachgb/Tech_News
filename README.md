# TechNewsScraper

Coletor de notícias e artigos de tecnologia desenvolvido em C#/.NET. A aplicação consulta o Hacker News e o Dev.to, remove artigos duplicados e gera relatórios locais em JSON, Markdown e HTML.

## Funcionalidades

- Consulta as 30 principais histórias do Hacker News.
- Consulta artigos do Dev.to pelas tags `dotnet`, `csharp`, `ai`, `webdev`, `devops` e `opensource`.
- Executa as consultas às fontes em paralelo.
- Ignora itens sem URL e remove URLs duplicadas.
- Ordena os artigos por pontuação nos relatórios.
- Gera tags automaticamente com o Gemini quando um artigo não possui tags.
- Mantém um histórico diário dos relatórios Markdown.
- Armazena a contagem de tags em um banco SQLite.
- Gera um gráfico HTML com as 15 tags mais utilizadas.
- Mantém somente os últimos 30 dias no histórico JSON e nos arquivos Markdown.
- Cria um backup com sufixo `.bak-<timestamp>` quando o JSON existente não pode ser lido.

## Requisitos

- .NET 10 SDK
- Acesso à internet para consultar as APIs do Hacker News e do Dev.to
- Uma chave da API do Gemini caso a geração automática de tags seja desejada

## Configuração da IA

Defina `GEMINI_API_KEY` no ambiente antes de executar a aplicação:

### PowerShell

```powershell
$env:GEMINI_API_KEY = "sua-chave-do-gemini"
```

### Windows (persistente para o usuário)

```powershell
[Environment]::SetEnvironmentVariable("GEMINI_API_KEY", "sua-chave-do-gemini", "User")
```

Se a variável não estiver definida, a aplicação ainda coleta os artigos, mas as requisições de geração de tags ao Gemini não poderão ser concluídas.

## Como executar

Execute os comandos na pasta que contém `TechNewsScraper.csproj`:

```powershell
dotnet restore
dotnet run
```

Para gerar uma versão otimizada:

```powershell
dotnet run --configuration Release
```

Ao concluir, o programa informa no terminal a quantidade de artigos coletados e salva os arquivos na pasta `output/`.

## Arquivos gerados

```text
output/
├── news.json                 # histórico estruturado dos últimos 30 dias
├── news.md                   # relatório da execução atual
├── NewsHistory/              # relatórios Markdown arquivados por data
└── View/
    ├── news.db               # banco SQLite com as contagens de tags
    └── .html                  # gráfico das 15 tags mais utilizadas
```

O arquivo `output/View/.html` pode ser aberto diretamente no navegador. O gráfico usa o Chart.js via CDN e, por isso, precisa de acesso à internet para carregar a biblioteca.

## Estrutura principal

```text
Program.cs                         # ponto de entrada e fluxo da execução
Services/HackerNews.cs             # integração com a API do Hacker News
Services/Dev-to.cs                 # integração com a API do Dev.to
Models/                            # modelos dos relatórios e artigos
Writer/Writer.cs                   # escrita dos relatórios JSON e Markdown
statistics/statistic.cs            # persistência e leitura das estatísticas
statistics/HtmlCharts.cs           # geração do gráfico HTML
Clear.cs                           # limpeza de dados com mais de 30 dias
```

## Estatísticas

As contagens de tags são gravadas em `output/View/news.db` durante a execução. Também é possível obter as contagens acumuladas a partir de um arquivo JSON usando `Statistic.PrintStatistics`:

```csharp
var stats = Estatisticas.Statistic.PrintStatistics("output/news.json");

foreach (var (tag, quantidade) in stats)
{
    Console.WriteLine($"{tag}: {quantidade}");
}
```

## Fontes de dados

- [Hacker News API](https://github.com/HackerNews/API)
- [Dev.to API](https://developers.forem.com/api)
- [Google Gemini API](https://ai.google.dev/gemini-api/docs)

## Objetivo

Facilitar o acompanhamento diário de notícias e artigos técnicos, reunindo conteúdos relevantes da comunidade em relatórios pesquisáveis e em um resumo visual das principais tags.
