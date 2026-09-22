using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
namespace Service.Admin.TVShows.TVShowEpisode;

public class TVShowEpisodeListDto
{
    public int Id { get; set; }

    public int SeasonNumber { get; set; }

    public int EpisodeNumber { get; set; }

    public string Name { get; set; } = string.Empty;

    public string ImagePath { get; set; } = string.Empty;

    public string DownloadLink { get; set; } = string.Empty;

    public int TVShowId { get; set; }

    public string TVShowTitle { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}


public class TVShowEpisodeCreateDto
{
    [Required(ErrorMessage = "انتخاب تی‌وی شو الزامی است.")]
    public int TVShowId { get; set; }

    [Range(1, int.MaxValue,
        ErrorMessage = "شماره فصل باید حداقل 1 باشد.")]
    public int SeasonNumber { get; set; } = 1;

    [Range(1, int.MaxValue,
        ErrorMessage = "شماره قسمت باید حداقل 1 باشد.")]
    public int EpisodeNumber { get; set; } = 1;

    [Required(ErrorMessage = "نام قسمت الزامی است.")]
    [MaxLength(100,
        ErrorMessage = "نام قسمت نمی‌تواند بیشتر از 100 کاراکتر باشد.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "تصویر قسمت الزامی است.")]
    public IFormFile? Image { get; set; }

    [Required(ErrorMessage = "لینک دانلود الزامی است.")]
    [MaxLength(1000,
        ErrorMessage = "لینک دانلود نمی‌تواند بیشتر از 1000 کاراکتر باشد.")]
    public string DownloadLink { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public List<TVShowEpisodeTVShowItemDto> TVShows { get; set; } = new();
}


public class TVShowEpisodeEditDto
{
    public int Id { get; set; }

    public int TVShowId { get; set; }

    public string TVShowTitle { get; set; } = string.Empty;

    [Range(1, int.MaxValue,
        ErrorMessage = "شماره فصل باید حداقل 1 باشد.")]
    public int SeasonNumber { get; set; }

    [Range(1, int.MaxValue,
        ErrorMessage = "شماره قسمت باید حداقل 1 باشد.")]
    public int EpisodeNumber { get; set; }

    [Required(ErrorMessage = "نام قسمت الزامی است.")]
    [MaxLength(100,
        ErrorMessage = "نام قسمت نمی‌تواند بیشتر از 100 کاراکتر باشد.")]
    public string Name { get; set; } = string.Empty;

    public string ExistingImagePath { get; set; } = string.Empty;

    public IFormFile? Image { get; set; }

    [Required(ErrorMessage = "لینک دانلود الزامی است.")]
    [MaxLength(1000,
        ErrorMessage = "لینک دانلود نمی‌تواند بیشتر از 1000 کاراکتر باشد.")]
    public string DownloadLink { get; set; } = string.Empty;

    public bool IsActive { get; set; }
}


public class TVShowEpisodeDetailsDto
{
    public int Id { get; set; }

    public int TVShowId { get; set; }

    public string TVShowTitle { get; set; } = string.Empty;

    public int SeasonNumber { get; set; }

    public int EpisodeNumber { get; set; }

    public string Name { get; set; } = string.Empty;

    public string ImagePath { get; set; } = string.Empty;

    public string DownloadLink { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}


public class TVShowEpisodeTVShowItemDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;
}