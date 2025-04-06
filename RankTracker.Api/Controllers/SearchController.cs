using Microsoft.AspNetCore.Mvc;
using RankTracker.Api.Contracts.Requests;
using RankTracker.Core;

namespace RankTracker.Api.Controllers;

[ApiController]
public class SearchController : ControllerBase
{
    private readonly ISearchService _searchService;

    public SearchController(ISearchService searchService)
    {
        _searchService = searchService;
    }


    [HttpGet]
    [Route(Routes.SearchHistory)]
    public async Task<IActionResult> GetSearchHistoryAsync()
    {
        var history = await _searchService.GetSearchHistoryAsync();
        return Ok(history);
    }

    [HttpPost]
    [Route(Routes.Search)]
    public async Task<IActionResult> SearchPositionsAsync([FromBody] SearchRequest request)
    {
        var result = await _searchService.SearchPositionsAsync(request.Query, request.Url, request.SearchEngine);
        return Ok(result);
    }
}