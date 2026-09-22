using DataBase.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace DataBase.EntityConfig;

public class BlogCategoryConfig : IEntityTypeConfiguration<BlogCategory>
{
    public void Configure(EntityTypeBuilder<BlogCategory> builder)
    {
        builder.ToTable("BlogCategories");

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Slug)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(x => x.DisplayOrder)
            .HasDefaultValue(0);

        builder.HasIndex(x => x.Name)
            .IsUnique();

        builder.HasIndex(x => x.Slug)
            .IsUnique();
    }
}
