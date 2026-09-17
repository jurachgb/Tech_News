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

## Como funciona

O projeto usa `HttpClient` para consultar as APIs públicas das fontes e organiza os dados em modelos compartilhados. Em seguida, o programa grava os resultados em arquivos na pasta `output`.

## Requisitos

- .NET 10 SDK
- Conexão com a internet

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

## Objetivo

O objetivo do projeto é montar um resumo automatizado de notícias e artigos técnicos para facilitar acompanhamento de tendências e conteúdos relevantes da comunidade.
