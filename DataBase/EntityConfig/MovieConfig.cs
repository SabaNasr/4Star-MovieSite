using DataBase.Entity;
using Microsoft.EntityFrameworkCore;
namespace DataBase.EntityConfig;
public class MovieConfig : IEntityTypeConfiguration<Movie>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Movie> builder)
    {
        builder.ToTable("Movies");


        // ==================================================
        // اطلاعات اصلی Movie
        // ==================================================

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


        // ==================================================
        // SEO
        // ==================================================

        builder.Property(x => x.MetaTitle)
            .HasMaxLength(250);

        builder.Property(x => x.MetaDescription)
            .HasMaxLength(500);

        builder.Property(x => x.MetaKeywords)
            .HasMaxLength(500);

        builder.Property(x => x.Slug)
            .HasMaxLength(250);

        builder.Property(x => x.CanonicalUrl)
            .HasMaxLength(500);

        builder.Property(x => x.OgTitle)
            .HasMaxLength(250);

        builder.Property(x => x.OgDescription)
            .HasMaxLength(500);

        builder.Property(x => x.OgImage)
            .HasMaxLength(500);

        builder.Property(x => x.TwitterCard)
            .HasMaxLength(50);

        builder.Property(x => x.NoIndex)
            .HasDefaultValue(false);

        builder.Property(x => x.NoFollow)
            .HasDefaultValue(false);


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

        // ==================================================
        // Indexes
        // ==================================================

        builder.HasIndex(x => x.Title);

        builder.HasIndex(x => x.Year);

        builder.HasIndex(x => x.Slug)
            .IsUnique()
            .HasFilter("[Slug] IS NOT NULL");

    }
}
