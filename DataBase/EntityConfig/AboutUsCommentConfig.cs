using DataBase.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace DataBase.EntityConfig;

public class AboutUsCommentConfig : IEntityTypeConfiguration<AboutUsComment>
{
    public void Configure(EntityTypeBuilder<AboutUsComment> builder)
    {
        builder.ToTable("AboutUsComments");

        builder.Property(x => x.FullName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.JobTitle)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.Content)
            .IsRequired()
            .HasColumnType("nvarchar(max)");

        builder.Property(x => x.StarRating)
            .IsRequired();

        builder.Property(x => x.IsApproved)
            .HasDefaultValue(false);

        builder.HasIndex(x => new
        {
            x.AboutUsId,
            x.IsApproved
        });
    }
}
