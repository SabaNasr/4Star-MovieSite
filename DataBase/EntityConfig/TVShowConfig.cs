using DataBase.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace DataBase.EntityConfig;

public class TVShowConfig : IEntityTypeConfiguration<TVShow>
{
    public void Configure(EntityTypeBuilder<TVShow> builder)
    {
        // ==================================================
        // ارث بری
        // ==================================================
        builder.HasBaseType<Movie>();

        // ==================================================
        // رایطه با فصلها
        // ==================================================

        builder.HasMany(x => x.Episodes)
            .WithOne(x => x.TVShow)
            .HasForeignKey(x => x.TVShowId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
