using System.Reflection;
using Microsoft.EntityFrameworkCore;
using RankTracker.DataAccess.Entities;

namespace RankTracker.DataAccess.Context;

public class RankTrackerDbContext : DbContext
{
    public DbSet<SearchLogEntity> SearchLogs { get; set; }

    public RankTrackerDbContext(DbContextOptions<RankTrackerDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("rank_tracker");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
