using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RankTracker.Api.Contracts.Responses;
using RankTracker.Core.Enums;
using RankTracker.Core.Factories;
using RankTracker.Core.Models;
using RankTracker.DataAccess.Entities;
using RankTracker.DataAccess.Repository;
using RankTracker.Infrastructure.Constants;
using RankTracker.Infrastructure.Options;


namespace RankTracker.Core.Services;

public class SearchService : ISearchService
{
    private readonly IHtmlParserFactory _parserFactory;
    private readonly ISearchLogRepository _searchLogRepository;
    private readonly SearchOptions _searchOptions;
    private readonly ILogger<SearchService> _logger;

    public SearchService(
        IHtmlParserFactory parserFactory,
        ISearchLogRepository searchLogRepository,
        IOptions<SearchOptions> searchOptions,
        ILogger<SearchService> logger)
    {
        _parserFactory = parserFactory;
        _searchLogRepository = searchLogRepository;
        _searchOptions = searchOptions.Value;
        _logger = logger;
    }

    public async Task<SearchResult> SearchPositionsAsync(string query, string url, string selectedEngine)
    {
        try
        {
            if (!Enum.TryParse<SearchEngine>(selectedEngine, true, out var searchEngine))
            {
                _logger.LogWarning("Unsupported search engine specified: {SelectedEngine}. Defaulting to Google.", selectedEngine);
                searchEngine = SearchEngine.Google;
            }

            var searchUrl = searchEngine switch
            {
                SearchEngine.Google => $"{_searchOptions.Google.BaseUrl}?num={_searchOptions.Google.NumResults}&q={Uri.EscapeDataString(query)}",
                SearchEngine.Bing => $"{_searchOptions.Bing.BaseUrl}?q={Uri.EscapeDataString(query)}&count={_searchOptions.Bing.NumResults}",
                _ => throw new ArgumentException("Unsupported search engine", nameof(selectedEngine))
            };

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("User-Agent", HttpHeadersConstants.UserAgent);
            httpClient.DefaultRequestHeaders.Add("Accept", HttpHeadersConstants.Accept);

            var htmlContent = await httpClient.GetStringAsync(searchUrl);

            var parser = _parserFactory.GetParser(searchEngine);
            var positions = parser.ParsePositions(htmlContent, url);

            await _searchLogRepository.AddAsync(new SearchLogEntity
            {
                Query = query,
                Url = url,
                SearchEngine = selectedEngine,
                Positions = string.Join(",", positions),
                CreatedAt = DateTime.UtcNow
            });

            return new SearchResult { Positions = positions };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting search results for query '{Query}' and url '{Url}'", query, url);
            throw;
        }
    }

    public async Task<List<SearchLogResponse>> GetSearchHistoryAsync()
    {
        try
        {
            var logs = await _searchLogRepository.GetLatestLogsAsync(20);

            return logs.Select(log => new SearchLogResponse
            {
                Id = log.Id,
                Query = log.Query,
                Url = log.Url,
                SearchEngine = log.SearchEngine,
                Positions = log.Positions,
                CreatedAt = log.CreatedAt
            }).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving search history.");
            throw;
        }
    }

}
