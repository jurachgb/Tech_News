# TechNewsScraper

Coletor de notícias de tecnologia em C# para reunir artigos relevantes de fontes populares como Hacker News e Dev.to e exportá-los em arquivos JSON e Markdown.

## Funcionalidades

- Busca as principais histórias do Hacker News
- Busca artigos do Dev.to por tags como .NET, C#, IA, webdev e DevOps
- Remove URLs duplicadas
- Ordena os resultados por pontuação
- Gera relatórios em:
  - `output/news.json`
  - `output/news.md`

- Geração automática de tags para artigos usando um modelo generativo (Gemini) quando as tags não estão presentes
- Arquivamento automático dos relatórios Markdown em `output/NewsHistory`
- Geração de estatísticas de tags a partir do histórico (veja `statistics/statistic.cs`)
- Backup automático do arquivo JSON existente (.bak-<timestamp>) caso o arquivo esteja corrompido

## Como funciona

O projeto usa `HttpClient` para consultar as APIs públicas das fontes e organiza os dados em modelos compartilhados. Em seguida, o programa grava os resultados em arquivos na pasta `output`.

## Requisitos

- .NET 10 SDK
- Conexão com a internet

### Tagging por IA

- Para gerar tags automaticamente, a aplicação usa a API generativa (Gemini). Defina a variável de ambiente `GEMINI_API_KEY` com sua chave antes de executar o programa.
- Há uma pequena pausa entre requisições ao serviço de IA para respeitar limites do plano gratuito.

##  Como executar

No diretório do projeto, rode:

```bash
dotnet run
```

Ao final da execução, os arquivos serão criados em:

```text
output/
├── news.json
└── news.md
```

## Estrutura principal

- `Program.cs` — fluxo principal da aplicação
- `Services/HackerNews.cs` — integração com a API do Hacker News
- `Services/Dev-to.cs` — integração com a API do Dev.to
- `Output/Writer.cs` — geração dos relatórios em JSON e Markdown
- `Models/Artigo.cs` — modelos de dados

## Observações

- A aplicação usa um User-Agent para chamadas HTTP
- A coleta é feita em paralelo para otimizar a velocidade
- Itens sem URL são ignorados para evitar entradas inválidas

- Estatísticas: existe um utilitário em `statistics/statistic.cs` que expõe `Estatisticas.Statistic.PrintStatistics(string file)` e retorna um dicionário de contagem de tags. Exemplo de uso em C#:

```csharp
var stats = Estatisticas.Statistic.PrintStatistics("output/news.json");
foreach(var kv in stats) Console.WriteLine($"{kv.Key}: {kv.Value}");
```

## Objetivo

O objetivo do projeto é montar um resumo automatizado de notícias e artigos técnicos para facilitar acompanhamento de tendências e conteúdos relevantes da comunidade.
