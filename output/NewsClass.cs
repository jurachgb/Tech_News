namespace ClassCreator;
using Models;

public static class NewsClass
{
    public static void CreateTag(Models.NewsArticle article)
    {
        var tags = new Dictionary<string, List<string>>
        {
            { "C#", new List<string> { "C#", "DOTNET", "DOTNETCORE", "ASPNET", ".NET", "DOTNET5" ,"CSHARP"} },
            { "C++", new List<string> { "C++", "CPP" } },
            { "JavaScript", new List<string> { "JAVASCRIPT", "NODEJS", "REACTJS" ,"REACT"} },
            { "Python", new List<string> { "PYTHON", "DJANGO", "FLASK" } },
            { "Java", new List<string> { "JAVA", "SPRING", "HIBERNATE" } },
            { "Go", new List<string> { "GOLANG" } },
            { "Rust", new List<string> { "RUST" } },
            { "TypeScript", new List<string> { "TYPESCRIPT" } },
            { "Kotlin", new List<string> { "KOTLIN" } },
            { "Swift", new List<string> { "SWIFT" } },
            { "PHP", new List<string> { "PHP", "LARAVEL", "SYMFONY" } },
            { "AI", new List<string> { "AI ", "LLM", "OPENAI", "CHATGPT","CHAT-GPT","GPT","CLAUDE", "ANTHROPIC", "MACHINELEARNING","GEMINI" } },
            { "DevOps", new List<string> { "DEVOPS", "DOCKER", "TERRAFORM", "GITOPS", "IAC", "CICD", "LINUX", "CLOUD", "WSL" } },
            { "Security", new List<string> { "SECURITY", "CYBERSECURITY", "GITHUBACTIONS" } },
            { "WebDev", new List<string> { "WEBDEV", "SAAS", "UX", "A11Y", "DESIGNSYSTEM" } },
            { "Database", new List<string> { "DATABASE", "MONGODB", "NOSQL", "S3", "MINIO" } },
            { "GameDev", new List<string> { "GAMEDEV", "UNITY3D", "TTRPG" } },
            { "Career", new List<string> { "CAREER", "DISCUSS", "PROGRAMMING", "LEARNING" } },
            { "OpenSource", new List<string> { "OPENSOURCE", "SHOWDEV" } },
            { "Architecture", new List<string> { "ARCHITECTURE", "BACKEND", "PERFORMANCE", "DEBUGGING" } },
            { "Mobile", new List<string> { "MOBILE", "ANDROID", "IOS" } },
            { "Cloud", new List<string> { "CLOUD", "AWS", "AZURE", "GCP" } },
            {"Games" ,new List<string> { "GAMES", "GAMING", "VIDEOGAMES" , "GAMER" ,"STEAM"} }
        };
       
        if ( article.Tag.Count == 0)
        {
            string titulo= article.Titulo.ToUpper().Trim();
            foreach (var k in tags)
            {
                if (k.Value.Any(x => titulo.Contains(x)) && !article.Tag.Contains(k.Key))
                {
                    article.Tag.Add(k.Key);
                }
            }
        }
    }
}   
