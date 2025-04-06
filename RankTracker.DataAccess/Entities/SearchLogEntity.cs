namespace RankTracker.DataAccess.Entities;

public class SearchLogEntity
{
    public long Id { get; set; }
    public required string Query { get; set; }
    public required string Url { get; set; }
    public required string SearchEngine { get; set; }
    public required string Positions { get; set; }
    public DateTime CreatedAt { get; set; }
}
