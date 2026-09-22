using DataBase.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace DataBase.EntityConfig;

public class UserQuestionConfig : IEntityTypeConfiguration<UserQuestion>
{
    public void Configure(EntityTypeBuilder<UserQuestion> builder)
    {
        builder.ToTable("UserQuestions");

        builder.Property(x => x.FullName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(x => x.Message)
            .IsRequired()
            .HasColumnType("nvarchar(max)");

        builder.HasIndex(x => x.CreatedAt);
    }
}
