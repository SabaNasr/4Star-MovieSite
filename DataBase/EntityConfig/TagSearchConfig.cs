using DataBase.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace DataBase.EntityConfig;

public class TagSearchConfig : IEntityTypeConfiguration<TagSearch>
{
    public void Configure(EntityTypeBuilder<TagSearch> builder)
    {
        builder.ToTable("TagSearches");

        builder.Property(x => x.Title).IsRequired().IsUnicode().HasMaxLength(150);
        builder.Property(x => x.Slug).IsRequired().HasMaxLength(150);

        builder.HasIndex(x => x.Slug).IsUnique();

        //DataSeed های نمونه
        builder.HasData(
           new TagSearch{Id = 1,Title = "فیلم جدید",Slug = "new-movies",CreatedAt = new DateTime(2024, 1, 1),IsDeleted = false},
           new TagSearch{Id = 2,Title = "سریال جدید",Slug = "new-series",CreatedAt = new DateTime(2024, 1, 1),IsDeleted = false},
           new TagSearch{Id = 3,Title = "فیلم محبوب",Slug = "popular-movies",CreatedAt = new DateTime(2024, 1, 1),IsDeleted = false},
           new TagSearch{Id = 4,Title = "سریال محبوب",Slug = "popular-series",CreatedAt = new DateTime(2024, 1, 1),IsDeleted = false},
           new TagSearch{Id = 8,Title = "سریال خارجی",Slug = "foreign-series",CreatedAt = new DateTime(2024, 1, 1),IsDeleted = false},
           new TagSearch{Id = 12,Title = "کمدی",Slug = "comedy",CreatedAt = new DateTime(2024, 1, 1),IsDeleted = false},
           new TagSearch{Id = 13,Title = "درام",Slug = "drama",CreatedAt = new DateTime(2024, 1, 1),IsDeleted = false},
           new TagSearch{Id = 14,Title = "ترسناک",Slug = "horror",CreatedAt = new DateTime(2024, 1, 1),IsDeleted = false}
        );
    }
}
