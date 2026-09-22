using System.ComponentModel.DataAnnotations;
namespace Service.Admin.TVShows.TVShowDescription;

public class TVShowDescriptionListDto
{
    public int Id { get; set; }

    public int TVShowId { get; set; }

    public string TVShowTitle { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}


public class TVShowDescriptionCreateDto
{
    [Required(ErrorMessage = "انتخاب تی‌وی شو الزامی است.")]
    public int TVShowId { get; set; }

    [Required(ErrorMessage = "شرح کامل تی‌وی شو الزامی است.")]
    public string Content { get; set; } = string.Empty;

    public List<TVShowDescriptionTVShowItemDto> TVShows { get; set; }
        = new();
}


public class TVShowDescriptionEditDto
{
    public int Id { get; set; }

    public int TVShowId { get; set; }

    public string TVShowTitle { get; set; } = string.Empty;

    [Required(ErrorMessage = "شرح کامل تی‌وی شو الزامی است.")]
    public string Content { get; set; } = string.Empty;
}


public class TVShowDescriptionDetailsDto
{
    public int Id { get; set; }

    public int TVShowId { get; set; }

    public string TVShowTitle { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}


public class TVShowDescriptionTVShowItemDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;
}