using DataBase.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace DataBase.EntityConfig;

public class BlogPostConfig : IEntityTypeConfiguration<BlogPost>
{
    public void Configure(EntityTypeBuilder<BlogPost> builder)
    {
        builder.ToTable("BlogPosts");

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(x => x.Slug)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(x => x.ImagePath)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.ShortDescription)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(x => x.Content)
            .IsRequired()
            .HasColumnType("nvarchar(max)");

        builder.Property(x => x.PublishDate)
            .IsRequired();

        builder.Property(x => x.LikeCount)
            .HasDefaultValue(0);

        // رابطه چند به چند با دسته‌بندی
        builder.HasMany(x => x.Categories)
            .WithMany(x => x.BlogPosts);

        // رابطه یک به چند با نظرات
        builder.HasMany(x => x.Comments)
            .WithOne(x => x.BlogPost)
            .HasForeignKey(x => x.BlogPostId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.Title);

        builder.HasIndex(x => x.Slug)
            .IsUnique();

        builder.HasIndex(x => x.PublishDate);
    }
}
