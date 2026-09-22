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

    public List<TVShowAdditionalInfoTVShowItemDto> TVShows { get; set; } = new();
}

public class TVShowAdditionalInfoEditDto
{
    public int Id { get; set; }

    public int TVShowId { get; set; }

    public string TVShowTitle { get; set; } = string.Empty;

    [Required(ErrorMessage = "اطلاعات تکمیلی تی‌وی شو الزامی است.")]
    public string Content { get; set; } = string.Empty;
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
}

public class TVShowAdditionalInfoTVShowItemDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;
}