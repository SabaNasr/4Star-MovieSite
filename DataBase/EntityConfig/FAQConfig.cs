using DataBase.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace DataBase.EntityConfig;

public class FAQConfig : IEntityTypeConfiguration<FAQ>
{
    public void Configure(EntityTypeBuilder<FAQ> builder)
    {
        builder.ToTable("FAQs");

        builder.Property(x => x.Question)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.Answer)
            .IsRequired()
            .HasColumnType("nvarchar(max)");

        builder.Property(x => x.Order)
            .HasDefaultValue(0);

        builder.HasIndex(x => x.Order);
    }
}
