using System.ComponentModel.DataAnnotations;
namespace Service.Admin.TVShows.TVShowAdditionalInfo;

public class TVShowAdditionalInfoListDto
{
    public int Id { get; set; }

    public int TVShowId { get; set; }

    public string TVShowTitle { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}


public class TVShowAdditionalInfoCreateDto
{
    [Required(ErrorMessage = "انتخاب تی‌وی شو الزامی است.")]
    public int TVShowId { get; set; }

    [Required(ErrorMessage = "اطلاعات تکمیلی تی‌وی شو الزامی است.")]
    public string Content { get; set; } = string.Empty;

    public List<TVShowAdditionalInfoTVShowItemDto>TVShows{ get; set; } = new();
}


public class TVShowAdditionalInfoEditDto
{
    public int Id { get; set; }

    public int TVShowId { get; set; }

    public string TVShowTitle { get; set; } = string.Empty;

    [Required(ErrorMessage = "اطلاعات تکمیلی تی‌وی شو الزامی است.")]
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


public class TVShowAdditionalInfoDetailsDto
{
    public int Id { get; set; }

    public int TVShowId { get; set; }

    public string TVShowTitle { get; set; } = string.Empty;

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


public class TVShowAdditionalInfoTVShowItemDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;
}