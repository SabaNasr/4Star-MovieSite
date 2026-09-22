using DataBase.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace DataBase.EntityConfig;

public class TVShowEpisodeConfig : IEntityTypeConfiguration<TVShowEpisode>
{
    public void Configure(EntityTypeBuilder<TVShowEpisode> builder)
    {
        builder.ToTable("TVShowEpisodes");

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.ImagePath)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.DownloadLink)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(x => x.EpisodeNumber)
            .IsRequired();

        builder.HasIndex(x => new
        {
            x.TVShowId,
            x.EpisodeNumber
        })
        .IsUnique();
    }
}
