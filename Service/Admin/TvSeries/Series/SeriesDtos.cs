using DataBase.Enum;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
namespace Service.Admin.TvSeries.Series;

public class SeriesSelectionItemDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsSelected { get; set; }
}

public class SeriesCastMemberDto
{
    public int CastMemberId { get; set; }
    public CastType CastType { get; set; }
}

public class SeriesCastMemberSelectionDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string PhotoPath { get; set; } = string.Empty;
    public bool IsSelected { get; set; }
    public CastType CastType { get; set; }
}

public class SeriesCastMemberDetailsDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string PhotoPath { get; set; } = string.Empty;
    public CastType CastType { get; set; }
}


// ==================================================
// Series List
// ==================================================

public class SeriesListDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string PosterPath { get; set; } = string.Empty;

    public int Year { get; set; }

    public decimal ImdbRating { get; set; }

    public int DurationMinutes { get; set; }

    public long ViewCount { get; set; }

    public int SeasonCount { get; set; }

    public bool IsActive { get; set; }

    public bool IsArchived { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? ArchivedAt { get; set; }
}


// ==================================================
// Series Create
// ==================================================

public class SeriesCreateDto
{
    [Required(ErrorMessage = "عنوان سریال الزامی است.")]
    [MaxLength(250, ErrorMessage = "عنوان سریال نمی‌تواند بیشتر از 250 کاراکتر باشد.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "پوستر سریال الزامی است.")]
    public IFormFile? Poster { get; set; }

    [Required(ErrorMessage = "توضیح کوتاه الزامی است.")]
    [MaxLength(1000, ErrorMessage = "توضیح کوتاه نمی‌تواند بیشتر از 1000 کاراکتر باشد.")]
    public string ShortDescription { get; set; } = string.Empty;

    [MaxLength(1000, ErrorMessage = "لینک دانلود نمی‌تواند بیشتر از 1000 کاراکتر باشد.")]
    [Url(ErrorMessage = "لینک دانلود معتبر نیست.")]
    public string? DownloadLink { get; set; }

    [Range(1, 10000, ErrorMessage = "مدت زمان باید بین 1 تا 10000 دقیقه باشد.")]
    public int DurationMinutes { get; set; }

    [Range(1888, 2100, ErrorMessage = "سال وارد شده معتبر نیست.")]
    public int Year { get; set; }

    [Range(0, 10, ErrorMessage = "امتیاز IMDb باید بین 0 تا 10 باشد.")]
    public decimal ImdbRating { get; set; }

    [Range(0, long.MaxValue, ErrorMessage = "تعداد بازدید نمی‌تواند منفی باشد.")]
    public long ViewCount { get; set; }

    public List<int> SelectedGenreIds { get; set; } = new();

    public List<int> SelectedLanguageIds { get; set; } = new();

    public List<SeriesCastMemberDto> SelectedCastMembers { get; set; } = new();

    public List<Quality> AvailableQualities { get; set; } = new();

    public List<SeriesSelectionItemDto> Genres { get; set; } = new();

    public List<SeriesSelectionItemDto> Languages { get; set; } = new();

    public List<SeriesCastMemberSelectionDto> CastMembers { get; set; } = new();
}


// ==================================================
// Series Edit
// ==================================================

public class SeriesEditDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "عنوان سریال الزامی است.")]
    [MaxLength(250, ErrorMessage = "عنوان سریال نمی‌تواند بیشتر از 250 کاراکتر باشد.")]
    public string Title { get; set; } = string.Empty;

    public IFormFile? Poster { get; set; }

    public string ExistingPosterPath { get; set; } = string.Empty;

    [Required(ErrorMessage = "توضیح کوتاه الزامی است.")]
    [MaxLength(1000, ErrorMessage = "توضیح کوتاه نمی‌تواند بیشتر از 1000 کاراکتر باشد.")]
    public string ShortDescription { get; set; } = string.Empty;

    [MaxLength(1000, ErrorMessage = "لینک دانلود نمی‌تواند بیشتر از 1000 کاراکتر باشد.")]
    [Url(ErrorMessage = "لینک دانلود معتبر نیست.")]
    public string? DownloadLink { get; set; }

    [Range(1, 10000, ErrorMessage = "مدت زمان باید بین 1 تا 10000 دقیقه باشد.")]
    public int DurationMinutes { get; set; }

    [Range(1888, 2100, ErrorMessage = "سال وارد شده معتبر نیست.")]
    public int Year { get; set; }

    [Range(0, 10, ErrorMessage = "امتیاز IMDb باید بین 0 تا 10 باشد.")]
    public decimal ImdbRating { get; set; }

    [Range(0, long.MaxValue, ErrorMessage = "تعداد بازدید نمی‌تواند منفی باشد.")]
    public long ViewCount { get; set; }

    public List<int> SelectedGenreIds { get; set; } = new();

    public List<int> SelectedLanguageIds { get; set; } = new();

    public List<SeriesCastMemberDto> SelectedCastMembers { get; set; } = new();

    public List<Quality> AvailableQualities { get; set; } = new();

    public List<SeriesSelectionItemDto> Genres { get; set; } = new();

    public List<SeriesSelectionItemDto> Languages { get; set; } = new();

    public List<SeriesCastMemberSelectionDto> CastMembers { get; set; } = new();
}


// ==================================================
// Series Details
// ==================================================

public class SeriesDetailsDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string PosterPath { get; set; } = string.Empty;

    public string ShortDescription { get; set; } = string.Empty;

    public string? DownloadLink { get; set; }

    public int DurationMinutes { get; set; }

    public int Year { get; set; }

    public decimal ImdbRating { get; set; }

    public long ViewCount { get; set; }

    public List<string> Genres { get; set; } = new();

    public List<string> Languages { get; set; } = new();

    public List<Quality> AvailableQualities { get; set; } = new();

    public List<SeriesCastMemberDetailsDto> CastMembers { get; set; } = new();

    public List<SeriesSeasonItemDto> Seasons { get; set; } = new();

    public bool IsActive { get; set; }

    public bool IsArchived { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}


// ==================================================
// Season item for Series Details
// ==================================================

public class SeriesSeasonItemDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? DownloadLink { get; set; }

    public int EpisodeCount { get; set; }
}