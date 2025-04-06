namespace RankTracker.Core.Parsers;

public interface IHtmlParser
{
    /// <summary>
    /// Parses the provided HTML content to retrieve the positions where the specified URL appears in the search results.
    /// </summary>
    /// <param name="htmlContent">The HTML string containing the search results.</param>
    /// <param name="targetUrl">The target URL to search for.</param>
    /// <returns>A list of positions where the target URL is found.</returns>
    List<int> ParsePositions(string htmlContent, string targetUrl);
}
