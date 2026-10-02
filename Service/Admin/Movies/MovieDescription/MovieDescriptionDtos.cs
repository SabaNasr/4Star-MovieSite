using System.ComponentModel.DataAnnotations;
namespace Service.Admin.Movies.MovieDescription;

public class MovieDescriptionListDto
{
    public int Id { get; set; }

    public int MovieId { get; set; }

    public string MovieTitle { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}

public class MovieDescriptionCreateDto
{
    [Required(ErrorMessage = "انتخاب فیلم الزامی است.")]
    public int MovieId { get; set; }

    [Required(ErrorMessage = "شرح کامل فیلم الزامی است.")]
    public string Content { get; set; } = string.Empty;

    public List<MovieDescriptionMovieItemDto> Movies { get; set; } = new();
}

public class MovieDescriptionEditDto
{
    public int Id { get; set; }

    public int MovieId { get; set; }

    public string MovieTitle { get; set; } = string.Empty;

    [Required(ErrorMessage = "شرح کامل فیلم الزامی است.")]
    public string Content { get; set; } = string.Empty;


    // ==================================================
    // SEO
    // ==================================================

    public string? MetaTitle { get; set; }

    public string? MetaDescription { get; set; }

    public string? MetaKeywords { get; set; }

    public string? Slug { get; set; }

    public string? CanonicalUrl { get; set; }

    public string? OgTitle { get; set; }

    public string? OgDescription { get; set; }

    public string? OgImage { get; set; }

    public string? TwitterCard { get; set; }

    public bool NoIndex { get; set; }

    public bool NoFollow { get; set; }
}

public class MovieDescriptionDetailsDto
{
    public int Id { get; set; }

    public int MovieId { get; set; }

    public string MovieTitle { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }


    // ==================================================
    // SEO
    // ==================================================

    public string? MetaTitle { get; set; }

    public string? MetaDescription { get; set; }

    public string? MetaKeywords { get; set; }

    public string? Slug { get; set; }

    public string? CanonicalUrl { get; set; }

    public string? OgTitle { get; set; }

    public string? OgDescription { get; set; }

    public string? OgImage { get; set; }

    public string? TwitterCard { get; set; }

    public bool NoIndex { get; set; }

    public bool NoFollow { get; set; }
}

public class MovieDescriptionMovieItemDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;
}