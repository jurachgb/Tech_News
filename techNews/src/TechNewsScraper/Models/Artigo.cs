namespace Models;

public class NewsArticle
{
    public string Titulo {get;set;}= string.Empty;
    public string Url {get;set;} =string.Empty;
    public string Tonte {get;set;}=string.Empty;
    public int Score {get;set;}
    public DateTime Data {get;set;} =DateTime.UtcNow;
    public List<string> Tag {get;set;}=[];

}

public class News
{
    public DateTime DataNoticia {get;set;}= DateTime.UtcNow;
    public int NumeroArtigos {get;set;}
    public List<NewsArticle> Artigo {get;set;}=[];
}