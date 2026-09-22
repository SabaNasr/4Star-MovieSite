using System.ComponentModel.DataAnnotations;
namespace Service.Admin.TvSeries.Episodes;

public class EpisodeListDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string DownloadLink { get; set; } = string.Empty;
    public int SeasonId { get; set; }
    public string SeasonName { get; set; } = string.Empty;
    public int SeriesId { get; set; }
    public string SeriesTitle { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class EpisodeCreateDto
{
    [Required(ErrorMessage = "انتخاب فصل الزامی است.")]
    public int SeasonId { get; set; }

    [Required(ErrorMessage = "نام قسمت الزامی است.")]
    [MaxLength(100, ErrorMessage = "نام قسمت نمی‌تواند بیشتر از 100 کاراکتر باشد.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "لینک دانلود الزامی است.")]
    [MaxLength(1000, ErrorMessage = "لینک دانلود نمی‌تواند بیشتر از 1000 کاراکتر باشد.")]
    public string DownloadLink { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public List<EpisodeSeasonItemDto> Seasons { get; set; } = new();
}

public class EpisodeEditDto
{
    public int Id { get; set; }

    public int SeasonId { get; set; }

    public string SeasonName { get; set; } = string.Empty;

    public string SeriesTitle { get; set; } = string.Empty;

    [Required(ErrorMessage = "نام قسمت الزامی است.")]
    [MaxLength(100, ErrorMessage = "نام قسمت نمی‌تواند بیشتر از 100 کاراکتر باشد.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "لینک دانلود الزامی است.")]
    [MaxLength(1000, ErrorMessage = "لینک دانلود نمی‌تواند بیشتر از 1000 کاراکتر باشد.")]
    public string DownloadLink { get; set; } = string.Empty;

    public bool IsActive { get; set; }
}

public class EpisodeDetailsDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string DownloadLink { get; set; } = string.Empty;
    public int SeasonId { get; set; }
    public string SeasonName { get; set; } = string.Empty;
    public int SeriesId { get; set; }
    public string SeriesTitle { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class EpisodeSeasonItemDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SeriesTitle { get; set; } = string.Empty;
}