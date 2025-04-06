using System.ComponentModel.DataAnnotations;

namespace RankTracker.Api.Contracts.Requests;

public class SearchRequest
{
    [Required]
    public string Query { get; set; } = string.Empty;

    [Required]
    public string Url { get; set; } = string.Empty;

    public string SearchEngine { get; set; } = string.Empty;
}