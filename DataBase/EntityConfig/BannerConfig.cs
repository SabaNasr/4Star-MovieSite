using DataBase.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace DataBase.EntityConfig;

public class BannerConfig : IEntityTypeConfiguration<Banner>
{
    public void Configure(EntityTypeBuilder<Banner> builder)
    {
        builder.ToTable("Banners");

        builder.Property(x => x.Image)
               .IsRequired()
               .HasMaxLength(500)
               .IsUnicode(false);

        builder.Property(x => x.Alt)
               .HasMaxLength(250);

        builder.Property(x => x.Text)
               .HasMaxLength(500);

        builder.Property(x => x.Link)
               .HasMaxLength(500)
               .IsUnicode(false);

        builder.Property(x => x.Order)
               .IsRequired();

        builder.Property(x => x.IsActive)
               .HasDefaultValue(true);

        builder.HasOne(x => x.TagSearch)
               .WithMany()
               .HasForeignKey(x => x.TagSearchId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.Order, x.IsActive });
    }
}
