using RankTracker.Api.Contracts.Responses;
using RankTracker.Core.Models;

namespace RankTracker.Core;

public interface ISearchService
{
    Task<SearchResult> SearchPositionsAsync(string query, string url, string searchEngine);
    Task<List<SearchLogResponse>> GetSearchHistoryAsync();
}
