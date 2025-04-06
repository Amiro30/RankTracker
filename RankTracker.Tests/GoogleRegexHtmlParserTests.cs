using Microsoft.Extensions.Logging;
using Moq;
using RankTracker.Core.Parsers;

namespace RankTracker.Tests;

[TestFixture]
public class GoogleRegexHtmlParserTests
{
    [Test]
    public void ParsePositions_ShouldReturnExpectedPositions()
    {
        var mockLogger = new Mock<ILogger<GoogleRegexHtmlParser>>();
        var parser = new GoogleRegexHtmlParser(mockLogger.Object);
        var targetUrl = "www.youtube.com";

        List<int> positions = parser.ParsePositions("test", targetUrl);

        Assert.IsNotNull(positions);
        Assert.IsTrue(positions.Count > 0);
        Console.WriteLine(string.Join(", ", positions));
    }
}
