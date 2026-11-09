# Visualizações e Estatísticas (output/View)

Esta pasta contém os arquivos gerados para persistência e visualização estatística dos artigos coletados pelo **TechNewsScraper**.

## Conteúdo da Pasta

- **`news.db`**: Banco de dados SQLite contendo a tabela `CounTag`. A aplicação contabiliza as tags associadas a todos os artigos coletados na execução e salva a frequência de cada tag (`Id`, `Tag`, `Count`).
- **`chart.html` (ou `.html`)**: Página HTML autônoma gerada com [Chart.js](https://www.chartjs.org/) que renderiza um gráfico de pizza (pie chart) com as 15 tags mais frequentes dos últimos artigos coletados.
- **`README.md`**: Esta documentação em português sobre o propósito e funcionamento dos arquivos desta pasta.
- **`README.en.md`**: Versão em inglês da documentação desta pasta.

## Como Funciona o Fluxo

1. **Agrupamento e Contagem (`Estatisticas.Statistic`)**:
   - Os artigos coletados (do Hacker News e Dev.to) são processados para extrair e agrupar todas as tags.
   - O banco de dados SQLite `news.db` é inicializado (tabela `CounTag`).
   - Os registros anteriores são limpos e atualizados com as contagens da execução atual.

2. **Renderização Visual (`Graphics.HtmlCharts`)**:
   - Os dados são consultados do banco `news.db` ordenados de forma decrescente pela contagem.
   - As 15 tags mais utilizadas são selecionadas e embutidas em uma página HTML com um gráfico de pizza interativo utilizando Chart.js via CDN.

## Como Visualizar

Para visualizar as estatísticas gráficas, basta abrir o arquivo HTML gerado (ex.: `chart.html` ou `.html`) em qualquer navegador com acesso à internet (necessário para carregar o Chart.js via CDN).
