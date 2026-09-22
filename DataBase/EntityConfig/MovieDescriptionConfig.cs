using DataBase.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace DataBase.EntityConfig;

public class MovieDescriptionConfig : IEntityTypeConfiguration<MovieDescription>
{
    public void Configure(EntityTypeBuilder<MovieDescription> builder)
    {
        builder.ToTable("MovieDescriptions");

        // متن Rich Text می‌تواند طول زیادی داشته باشد
        builder.Property(x => x.Content)
            .IsRequired()
            .HasColumnType("nvarchar(max)");

        // هر فیلم فقط یک Description دارد
        builder.HasIndex(x => x.MovieId)
            .IsUnique();
    }
}
