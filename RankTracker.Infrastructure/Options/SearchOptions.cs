namespace RankTracker.Infrastructure.Options;

public class SearchOptions
{
    public EngineSettings Google { get; set; }
    public EngineSettings Bing { get; set; }
}

public class EngineSettings
{
    public string BaseUrl { get; set; }
    public int NumResults { get; set; }
}
