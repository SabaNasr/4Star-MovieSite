using System.ComponentModel.DataAnnotations;
namespace Service.Admin.TvSeries.SeriesDescription;

public class SeriesDescriptionListDto
{
    public int Id { get; set; }

    public int SeriesId { get; set; }

    public string SeriesTitle { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}


public class SeriesDescriptionCreateDto
{
    [Required(ErrorMessage = "انتخاب سریال الزامی است.")]
    public int SeriesId { get; set; }

    [Required(ErrorMessage = "شرح کامل سریال الزامی است.")]
    public string Content { get; set; } = string.Empty;

    public List<SeriesDescriptionSeriesItemDto>
        Series
    { get; set; } = new();
}


public class SeriesDescriptionEditDto
{
    public int Id { get; set; }

    public int SeriesId { get; set; }

    public string SeriesTitle { get; set; } = string.Empty;

    [Required(ErrorMessage = "شرح کامل سریال الزامی است.")]
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


public class SeriesDescriptionDetailsDto
{
    public int Id { get; set; }

    public int SeriesId { get; set; }

    public string SeriesTitle { get; set; } = string.Empty;

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


public class SeriesDescriptionSeriesItemDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;
}