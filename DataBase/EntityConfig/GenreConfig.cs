using DataBase.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace DataBase.EntityConfig;

public class GenreConfig : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder.ToTable("Genres");

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(x => x.Name)
            .IsUnique();
    }
}
