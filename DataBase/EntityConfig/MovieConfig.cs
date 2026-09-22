using DataBase.Entity;
using Microsoft.EntityFrameworkCore;
namespace DataBase.EntityConfig;
public class MovieConfig : IEntityTypeConfiguration<Movie>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Movie> builder)
    {
        builder.ToTable("Movies");

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(x => x.PosterPath)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.ShortDescription)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(x => x.DurationMinutes)
            .IsRequired();

        builder.Property(x => x.Year)
            .IsRequired();

        builder.Property(x => x.ImdbRating)
            .HasPrecision(3, 1);

        builder.Property(x => x.ViewCount)
            .HasDefaultValue(0);

        builder.Property(x => x.DownloadLink)
            .HasMaxLength(1000);

        builder.PrimitiveCollection(x => x.AvailableQualities)
            .ElementType()
            .HasConversion<string>();

        builder.HasMany(x => x.Genres)
            .WithMany(x => x.Movies);

        builder.HasMany(x => x.Languages)
            .WithMany(x => x.Movies);

        builder.HasMany(x => x.CastMembers)
            .WithMany(x => x.Movies)
            .UsingEntity<Dictionary<string, object>>(
                "MovieCastMembers",
                right => right
                    .HasOne<CastMember>()
                    .WithMany()
                    .HasForeignKey("CastMemberId")
                    .OnDelete(DeleteBehavior.Cascade),
                left => left
                    .HasOne<Movie>()
                    .WithMany()
                    .HasForeignKey("MovieId")
                    .OnDelete(DeleteBehavior.Cascade),
                join =>
                {
                    join.HasKey("MovieId", "CastMemberId");

                    join.Property<DataBase.Enum.CastType>("CastType")
                        .HasConversion<string>()
                        .IsRequired();

                    join.ToTable("MovieCastMembers");
                });

        builder.HasMany(x => x.Reviews)
            .WithOne(x => x.Movie)
            .HasForeignKey(x => x.MovieId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Description)
            .WithOne(x => x.Movie)
            .HasForeignKey<MovieDescription>(x => x.MovieId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.AdditionalInfo)
            .WithOne(x => x.Movie)
            .HasForeignKey<MovieAdditionalInfo>(x => x.MovieId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasDiscriminator<string>("Discriminator")
            .HasValue<Movie>("Movie")
            .HasValue<Series>("Series")
            .HasValue<TVShow>("TVShow");

        builder.HasIndex(x => x.Title);

        builder.HasIndex(x => x.Year);
    }
}
