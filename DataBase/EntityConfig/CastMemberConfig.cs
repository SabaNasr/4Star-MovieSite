using DataBase.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace DataBase.EntityConfig;
public class CastMemberConfig : IEntityTypeConfiguration<CastMember>
{
    public void Configure(EntityTypeBuilder<CastMember> builder)
    {
        builder.ToTable("CastMembers");

        builder.Property(x => x.FullName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.PhotoPath)
            .HasMaxLength(500);

        builder.HasIndex(x => x.FullName);
    }
}
