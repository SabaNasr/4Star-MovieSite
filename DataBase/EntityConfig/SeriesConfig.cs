using DataBase.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace DataBase.EntityConfig;

public class SeriesConfig : IEntityTypeConfiguration<Series>
{
    public void Configure(EntityTypeBuilder<Series> builder)
    {
        // ==================================================
        // ارث بری
        // ==================================================

        builder.HasBaseType<Movie>();

        // ==================================================
        // رایطه با فصلها
        // ==================================================

        builder.HasMany(x => x.Seasons)
            .WithOne(x => x.Series)
            .HasForeignKey(x => x.SeriesId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
