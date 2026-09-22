using DataBase.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace DataBase.EntityConfig;

public class EpisodeConfig : IEntityTypeConfiguration<Episode>
{
    public void Configure(EntityTypeBuilder<Episode> builder)
    {
        builder.ToTable("Episodes");


        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.DownloadLink)
            .IsRequired()
            .HasMaxLength(1000);


        builder.HasIndex(x => new
        {
            x.SeasonId,
            x.Name
        })
        .IsUnique();
    }
}
