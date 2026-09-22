using DataBase.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace DataBase.EntityConfig;

public class LanguageConfig : IEntityTypeConfiguration<Language>
{
    public void Configure(EntityTypeBuilder<Language> builder)
    {
        builder.ToTable("Languages");

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(x => x.Name)
            .IsUnique();
    }
}
