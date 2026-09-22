using DataBase.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace DataBase.EntityConfig;

public class TermsAndConditionsConfig : IEntityTypeConfiguration<TermsAndConditions>
{
    public void Configure(EntityTypeBuilder<TermsAndConditions> builder)
    {
        builder.ToTable("TermsAndConditions");

        builder.Property(x => x.Content)
            .IsRequired()
            .HasColumnType("nvarchar(max)");
    }
}
