# Histórico de Notícias

Esta pasta guarda os relatórios Markdown de dias anteriores.

## Como funciona

- O relatório do dia atual fica em `output/news.md`.
- Quando o scraper é executado em um novo dia, o relatório anterior é movido para esta pasta.
- Cada arquivo recebe o nome `noticia-dd-MM-yyyy.md`, por exemplo: `noticia-24-09-2026.md`.
- O relatório atual continua sendo substituído a cada execução do mesmo dia.
- Se já existir um arquivo histórico para a data anterior, ele não é substituído.

Assim, `news.md` sempre mostra as notícias mais recentes, enquanto esta pasta mantém os relatórios dos dias passados.
