using System.ComponentModel.DataAnnotations;
namespace Service.Admin.TvSeries.SeriesAdditionalInfo;

public class SeriesAdditionalInfoListDto
{
    public int Id { get; set; }

    public int SeriesId { get; set; }

    public string SeriesTitle { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}


public class SeriesAdditionalInfoCreateDto
{
    [Required(ErrorMessage = "انتخاب سریال الزامی است.")]
    public int SeriesId { get; set; }

    [Required(ErrorMessage = "اطلاعات تکمیلی سریال الزامی است.")]
    public string Content { get; set; } = string.Empty;

    public List<SeriesAdditionalInfoSeriesItemDto>
        Series
    { get; set; } = new();
}


public class SeriesAdditionalInfoEditDto
{
    public int Id { get; set; }

    public int SeriesId { get; set; }

    public string SeriesTitle { get; set; } = string.Empty;

    [Required(ErrorMessage = "اطلاعات تکمیلی سریال الزامی است.")]
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


public class SeriesAdditionalInfoDetailsDto
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


public class SeriesAdditionalInfoSeriesItemDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;
}