using DataBase.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace DataBase.EntityConfig;

public class PrivacyPolicyConfig : IEntityTypeConfiguration<PrivacyPolicy>
{
    public void Configure(EntityTypeBuilder<PrivacyPolicy> builder)
    {
        builder.ToTable("PrivacyPolicies");

        builder.Property(x => x.Content)
            .IsRequired()
            .HasColumnType("nvarchar(max)");
    }
}
