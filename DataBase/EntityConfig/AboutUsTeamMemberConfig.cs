using DataBase.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace DataBase.EntityConfig;

public class AboutUsTeamMemberConfig : IEntityTypeConfiguration<AboutUsTeamMember>
{
    public void Configure(EntityTypeBuilder<AboutUsTeamMember> builder)
    {
        builder.ToTable("AboutUsTeamMembers");

        builder.Property(x => x.FullName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.JobTitle)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.ImagePath)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.Order)
            .HasDefaultValue(0);

        builder.HasMany(x => x.SocialLinks)
            .WithOne(x => x.TeamMember)
            .HasForeignKey(x => x.TeamMemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new
        {
            x.AboutUsId,
            x.Order
        });
    }
}
