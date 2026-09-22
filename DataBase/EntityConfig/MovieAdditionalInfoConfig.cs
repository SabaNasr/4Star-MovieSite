using DataBase.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace DataBase.EntityConfig;

public class MovieAdditionalInfoConfig : IEntityTypeConfiguration<MovieAdditionalInfo>
{
    public void Configure(EntityTypeBuilder<MovieAdditionalInfo> builder)
    {
        builder.ToTable("MovieAdditionalInfos");

        builder.Property(x => x.Content)
            .IsRequired()
            .HasColumnType("nvarchar(max)");

        // هر فیلم فقط یک AdditionalInfo دارد
        builder.HasIndex(x => x.MovieId)
            .IsUnique();
    }
}
