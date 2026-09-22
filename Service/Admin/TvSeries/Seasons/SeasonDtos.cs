using System.ComponentModel.DataAnnotations;
namespace Service.Admin.TvSeries.Seasons;

public class SeasonListDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? DownloadLink { get; set; }
    public int SeriesId { get; set; }
    public string SeriesTitle { get; set; } = string.Empty;
    public int EpisodeCount { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class SeasonCreateDto
{
    [Required(ErrorMessage = "انتخاب سریال الزامی است.")]
    public int SeriesId { get; set; }

    [Required(ErrorMessage = "نام فصل الزامی است.")]
    [MaxLength(100, ErrorMessage = "نام فصل نمی‌تواند بیشتر از 100 کاراکتر باشد.")]
    public string Name { get; set; } = string.Empty;

    [MaxLength(1000, ErrorMessage = "لینک دانلود نمی‌تواند بیشتر از 1000 کاراکتر باشد.")]
    public string? DownloadLink { get; set; }

    public bool IsActive { get; set; } = true;

    public List<SeasonSeriesItemDto> Series { get; set; } = new();
}

public class SeasonEditDto
{
    public int Id { get; set; }

    public int SeriesId { get; set; }

    public string SeriesTitle { get; set; } = string.Empty;

    [Required(ErrorMessage = "نام فصل الزامی است.")]
    [MaxLength(100, ErrorMessage = "نام فصل نمی‌تواند بیشتر از 100 کاراکتر باشد.")]
    public string Name { get; set; } = string.Empty;

    [MaxLength(1000, ErrorMessage = "لینک دانلود نمی‌تواند بیشتر از 1000 کاراکتر باشد.")]
    public string? DownloadLink { get; set; }

    public bool IsActive { get; set; }
}

public class SeasonDetailsDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? DownloadLink { get; set; }
    public int SeriesId { get; set; }
    public string SeriesTitle { get; set; } = string.Empty;
    public int EpisodeCount { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class SeasonSeriesItemDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
}