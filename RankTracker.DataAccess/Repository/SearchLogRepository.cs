using RankTracker.DataAccess.Context;
using RankTracker.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace RankTracker.DataAccess.Repository;

public class SearchLogRepository : ISearchLogRepository
{
    private readonly RankTrackerDbContext _dbContext;

    public SearchLogRepository(RankTrackerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(SearchLogEntity entity)
    {
        await _dbContext.SearchLogs.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<SearchLogEntity>> GetLatestLogsAsync(int count)
    {
        return await _dbContext.SearchLogs
            .OrderByDescending(x => x.CreatedAt)
            .Take(count)
            .ToListAsync();
    }

    public async Task<SearchLogEntity?> GetByIdAsync(long id)
    {
        return await _dbContext.SearchLogs.FindAsync(id);
    }
}
