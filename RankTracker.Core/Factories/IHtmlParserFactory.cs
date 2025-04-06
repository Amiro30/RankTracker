using RankTracker.Core.Enums;
using RankTracker.Core.Parsers;

namespace RankTracker.Core.Factories;

public interface IHtmlParserFactory
{
    IHtmlParser GetParser(SearchEngine engine);
}
