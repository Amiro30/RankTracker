using RankTracker.DataAccess.Entities;

namespace RankTracker.DataAccess.Repository;

public interface ISearchLogRepository
{
    Task AddAsync(SearchLogEntity entity);
    Task<List<SearchLogEntity>> GetLatestLogsAsync(int count);
    Task<SearchLogEntity?> GetByIdAsync(long id);
}
