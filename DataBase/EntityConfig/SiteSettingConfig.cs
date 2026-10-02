using DataBase.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace DataBase.EntityConfig;

public class SiteSettingConfig : IEntityTypeConfiguration<SiteSetting>
{
    public void Configure(EntityTypeBuilder<SiteSetting> builder)
    {
        builder.ToTable("SiteSettings");

        builder.Property(x => x.Key).IsRequired().HasMaxLength(150);
        builder.Property(x => x.Value).IsRequired().IsUnicode().HasMaxLength(1000);
        builder.Property(x => x.Group).HasMaxLength(100);
        builder.Property(x => x.Description).HasMaxLength(300).IsUnicode();

        builder.HasIndex(x => x.Key).IsUnique();

        builder.HasData(
            new SiteSetting
            {
                Id = 1,
                Key = "SiteTitle",
                Value = "4Star",
                Group = "General",
                Description = "عنوان اصلی سایت",
                CreatedAt = new DateTime(
                    2024, 1, 1, 0, 0, 0,
                    DateTimeKind.Utc),
                IsDeleted = false
            },

            new SiteSetting
            {
                Id = 2,
                Key = "SiteSlogan",
                Value = "مرجع فیلم و سریال",
                Group = "General",
                Description = "شعار کوتاه سایت",
                CreatedAt = new DateTime(
                    2024, 1, 1, 0, 0, 0,
                    DateTimeKind.Utc),
                IsDeleted = false
            },

            new SiteSetting
            {
                Id = 3,
                Key = "MetaDescription",
                Value = "دانلود و تماشای جدیدترین فیلم‌ها، سریال‌ها و TV Showها در 4Star.",
                Group = "SEO",
                Description = "توضیحات متا برای موتورهای جستجو",
                CreatedAt = new DateTime(
                    2024, 1, 1, 0, 0, 0,
                    DateTimeKind.Utc),
                IsDeleted = false
            }
        );
    }
}
