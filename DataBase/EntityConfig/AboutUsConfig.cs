using DataBase.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace DataBase.EntityConfig;

public class AboutUsConfig : IEntityTypeConfiguration<AboutUs>
{
    public void Configure(EntityTypeBuilder<AboutUs> builder)
    {
        builder.ToTable("AboutUs");

        builder.Property(x => x.ImagePath)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.Content)
            .IsRequired()
            .HasMaxLength(500)
            .HasColumnType("nvarchar(max)");

        builder.Property(x => x.CustomerCount)
            .HasDefaultValue(0);

        builder.Property(x => x.ActiveUserCount)
            .HasDefaultValue(0);

        builder.Property(x => x.TotalVideoCount)
            .HasDefaultValue(0);

        builder.Property(x => x.SubscriberCount)
            .HasDefaultValue(0);

        builder.Property(x => x.AwardCount)
            .HasDefaultValue(0);

        builder.HasMany(x => x.Comments)
            .WithOne(x => x.AboutUs)
            .HasForeignKey(x => x.AboutUsId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.TeamMembers)
            .WithOne(x => x.AboutUs)
            .HasForeignKey(x => x.AboutUsId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
