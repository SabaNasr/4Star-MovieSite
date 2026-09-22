using DataBase.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace DataBase.EntityConfig;

public class ContactUsConfig : IEntityTypeConfiguration<ContactUs>
{
    public void Configure(EntityTypeBuilder<ContactUs> builder)
    {
        builder.ToTable("ContactUs");

        builder.Property(x => x.FullName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(x => x.Phone)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(x => x.Message)
            .IsRequired()
            .HasColumnType("nvarchar(max)");

        builder.HasMany(x => x.Cards)
            .WithOne(x => x.ContactUs)
            .HasForeignKey(x => x.ContactUsId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
