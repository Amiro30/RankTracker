using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using RankTracker.DataAccess.Entities;

namespace RankTracker.DataAccess.Configuraitons;

public class SearchLogEntityConfiguration : IEntityTypeConfiguration<SearchLogEntity>
{
    public void Configure(EntityTypeBuilder<SearchLogEntity> builder)
    {
        builder.ToTable("search_logs", schema: "rank_tracker");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.Query).IsRequired().HasMaxLength(300);

        builder.Property(x => x.Url).IsRequired().HasMaxLength(500);

        builder.Property(x => x.SearchEngine).IsRequired().HasMaxLength(50);

        builder.Property(x => x.Positions).IsRequired().HasMaxLength(200);

        builder.Property(x => x.CreatedAt).IsRequired();
    }
}