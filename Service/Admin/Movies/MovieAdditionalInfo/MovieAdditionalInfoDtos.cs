using System.ComponentModel.DataAnnotations;
namespace Service.Admin.Movies.MovieAdditionalInfo;

public class MovieAdditionalInfoListDto
{
    public int Id { get; set; }

    public int MovieId { get; set; }

    public string MovieTitle { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}


public class MovieAdditionalInfoCreateDto
{
    [Required(ErrorMessage = "انتخاب فیلم الزامی است.")]
    public int MovieId { get; set; }

    [Required(ErrorMessage = "اطلاعات تکمیلی فیلم الزامی است.")]
    public string Content { get; set; } = string.Empty;

    public List<MovieAdditionalInfoMovieItemDto>
        Movies
    { get; set; } = new();
}


public class MovieAdditionalInfoEditDto
{
    public int Id { get; set; }

    public int MovieId { get; set; }

    public string MovieTitle { get; set; } = string.Empty;

    [Required(ErrorMessage = "اطلاعات تکمیلی فیلم الزامی است.")]
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


public class MovieAdditionalInfoDetailsDto
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


public class MovieAdditionalInfoMovieItemDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;
}