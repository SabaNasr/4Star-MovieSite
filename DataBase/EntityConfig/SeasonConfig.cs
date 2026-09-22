using DataBase.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace DataBase.EntityConfig;

public class SeasonConfig : IEntityTypeConfiguration<Season>
{
    public void Configure(EntityTypeBuilder<Season> builder)
    {
        builder.ToTable("Seasons");


        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.DownloadLink)
            .HasMaxLength(1000);

        builder.HasOne(x => x.Series)
            .WithMany(x => x.Seasons)
            .HasForeignKey(x => x.SeriesId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Episodes)
            .WithOne(x => x.Season)
            .HasForeignKey(x => x.SeasonId)
            .OnDelete(DeleteBehavior.Restrict);


        builder.HasIndex(x => new
        {
            x.SeriesId,
            x.Name
        })
        .IsUnique();
    }
}
