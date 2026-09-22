using DataBase.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace DataBase.EntityConfig;

public class ContactUsCardConfig : IEntityTypeConfiguration<ContactUsCard>
{
    public void Configure(EntityTypeBuilder<ContactUsCard> builder)
    {
        builder.ToTable("ContactUsCards");

        builder.Property(x => x.Icon)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.Content)
            .IsRequired()
            .HasColumnType("nvarchar(max)");

        builder.Property(x => x.DisplayOrder)
            .HasDefaultValue(0);

        builder.HasIndex(x => new
        {
            x.ContactUsId,
            x.DisplayOrder
        });
    }
}
