using DataBase.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace DataBase.EntityConfig;
public class ReviewConfig : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("Reviews");

        builder.Property(x => x.FullName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.Content)
            .IsRequired()
            .HasColumnType("nvarchar(max)");

        // Enum امتیاز به صورت عدد ذخیره می‌شود
        builder.Property(x => x.StarRating)
            .IsRequired();

        // پیش‌فرض: نظر ابتدا تایید نشده است
        builder.Property(x => x.IsApproved)
            .HasDefaultValue(false);

        // برای نمایش نظرات تایید شده
        builder.HasIndex(x => new
        {
            x.MovieId,
            x.IsApproved
        });
    }
}
