using DataBase.Enum;
namespace DataBase.Entity;

public class Movie : BaseEntity
{
    public string Title { get; set; } = string.Empty;

    public string? PosterPath { get; set; }

    public string? ShortDescription { get; set; }

    public int DurationMinutes { get; set; }

    public int Year { get; set; }

    public decimal ImdbRating { get; set; }

    public long ViewCount { get; set; }

    public List<Quality> AvailableQualities { get; set; } = new();

    public string? DownloadLink { get; set; }


    // =========================
    // SEO
    // =========================

    public string? MetaTitle { get; set; }

    public string? MetaDescription { get; set; }

    public string? MetaKeywords { get; set; }

    public string? Slug { get; set; }

    public string? CanonicalUrl { get; set; }

    public string? OgTitle { get; set; }

    public string? OgDescription { get; set; }

    public string? OgImage { get; set; }

    public string? TwitterCard { get; set; }

    public bool NoIndex { get; set; } = false;

    public bool NoFollow { get; set; } = false;


    // =========================
    // Relationships
    // =========================

    public List<Genre> Genres { get; set; } = new();

    public List<Language> Languages { get; set; } = new();

    public List<CastMember> CastMembers { get; set; } = new();

    public List<Review> Reviews { get; set; } = new();

    public MovieDescription? Description { get; set; }

    public MovieAdditionalInfo? AdditionalInfo { get; set; }
}