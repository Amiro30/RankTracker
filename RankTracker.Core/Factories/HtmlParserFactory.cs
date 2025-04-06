using RankTracker.Core.Enums;
using RankTracker.Core.Parsers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace RankTracker.Core.Factories;

public class HtmlParserFactory : IHtmlParserFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<HtmlParserFactory> _logger;

    public HtmlParserFactory(IServiceProvider serviceProvider, ILogger<HtmlParserFactory> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public IHtmlParser GetParser(SearchEngine engine)
    {
        switch (engine)
        {
            case SearchEngine.Google:
                return _serviceProvider.GetRequiredService<GoogleRegexHtmlParser>();
            case SearchEngine.Bing:
                return _serviceProvider.GetRequiredService<BingHtmlParser>();

            default:
                _logger.LogError("Unsupported search engine: {Engine}", engine);
                throw new ArgumentException("Unsupported search engine");
        }
    }
}
