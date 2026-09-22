using DataBase.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace DataBase.EntityConfig;

public class AboutUsSocialLinkConfig : IEntityTypeConfiguration<AboutUsSocialLink>
{
    public void Configure(EntityTypeBuilder<AboutUsSocialLink> builder)
    {
        builder.ToTable("AboutUsSocialLinks");

        builder.Property(x => x.Icon)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Url)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(x => x.Order)
            .HasDefaultValue(0);

        builder.HasIndex(x => new
        {
            x.TeamMemberId,
            x.Order
        });
    }
}
