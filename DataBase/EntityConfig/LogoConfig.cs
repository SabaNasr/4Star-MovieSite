using DataBase.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace DataBase.EntityConfig;

public class LogoConfig : IEntityTypeConfiguration<Logo>
{
    public void Configure(EntityTypeBuilder<Logo> builder)
    {
        builder.ToTable("Logos");

        builder.Property(x => x.MainLogoUrl)
               .HasMaxLength(500)
               .IsUnicode(false);

        builder.Property(x => x.FooterLogoUrl)
               .HasMaxLength(500)
               .IsUnicode(false);

        builder.Property(x => x.FaviconUrl)
               .HasMaxLength(500)
               .IsUnicode(false);

        //DataSeed پیش فرض لوگو

        builder.HasData(
         new Logo
         {
             Id = 1,
             MainLogoUrl = null,      
             FooterLogoUrl = null,
             FaviconUrl = null,

           
             CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
             IsDeleted = false
         }
     );
    }
}
