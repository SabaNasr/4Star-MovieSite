using DataBase.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace DataBase.EntityConfig;

public class BlogCommentConfig : IEntityTypeConfiguration<BlogComment>
{
    public void Configure(EntityTypeBuilder<BlogComment> builder)
    {
        builder.ToTable("BlogComments");

        builder.Property(x => x.FullName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.Content)
            .IsRequired()
            .HasColumnType("nvarchar(max)");

        builder.Property(x => x.IsApproved)
            .HasDefaultValue(false);

        builder.HasIndex(x => new
        {
            x.BlogPostId,
            x.IsApproved
        });
    }
}
