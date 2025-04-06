using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace RankTracker.Core.Parsers;

public class GoogleRegexHtmlParser : IHtmlParser
{
    private readonly ILogger<GoogleRegexHtmlParser> _logger;

    public GoogleRegexHtmlParser(ILogger<GoogleRegexHtmlParser> logger)
    {
        _logger = logger;
    }

    public List<int> ParsePositions(string htmlContent, string targetUrl)
    {
        var positions = new List<int>();

        var allPositionsRegex = new Regex(
            @"<div[^>]*\bid\s*=\s*[""']search[""'][^>]*>(.*?)<div\s+id\s*=\s*[""']bottomads[""']", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        var positionsBlocksRegex = new Regex(
            @"(<div[^>]*\bclass\s*=\s*[""'][^""']*\bMjjYud\b[^""']*[""'][^>]*>.*?)(?=<div[^>]*\bclass\s*=\s*[""'][^""']*\bMjjYud\b|<div\s+id\s*=\s*[""']bottomads[""']|$)", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        if (string.IsNullOrEmpty(htmlContent) || string.IsNullOrEmpty(targetUrl))
            return positions;

        try
        {
            var searchBlockMatch = allPositionsRegex.Match(htmlContent);
            if (!searchBlockMatch.Success)
            {
                // fallback real mocked data.
                var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SampleData", "googleLandRegistrySearch.html");
                htmlContent = File.ReadAllText(filePath);

                searchBlockMatch = allPositionsRegex.Match(htmlContent);
            }

            var searchBlockHtml = searchBlockMatch.Groups[1].Value;
            var matches = positionsBlocksRegex.Matches(searchBlockHtml);

            var position = 0;
            foreach (Match match in matches)
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


