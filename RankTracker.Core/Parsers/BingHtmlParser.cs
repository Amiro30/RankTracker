using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace RankTracker.Core.Parsers;

public class BingHtmlParser : IHtmlParser
{
    private readonly ILogger<BingHtmlParser> _logger;

    public BingHtmlParser(ILogger<BingHtmlParser> logger)
    {
        _logger = logger;
    }
    public List<int> ParsePositions(string htmlContent, string targetUrl)
    {
        var positions = new List<int>();

        var liRegex = new Regex(@"<li[^>]*class=[""']b_algo[""'][^>]*>(.*?)</li>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
        try
        {
            var liMatches = liRegex.Matches(htmlContent);

            var position = 0;
            foreach (Match match in liMatches)
            {
                position++;
                if (position > 100)
                    break;

                var resultHtml = match.Groups[1].Value;

                if (resultHtml.IndexOf(targetUrl, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    positions.Add(position);
                }
            }

            return positions;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while parsing positions.");
            throw;
        }
    }
}
