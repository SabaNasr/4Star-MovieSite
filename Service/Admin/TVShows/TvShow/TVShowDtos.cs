using DataBase.Enum;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
namespace Service.Admin.TVShows.TvShow;

public class TVShowListDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string PosterPath { get; set; } = string.Empty;

    public int Year { get; set; }

    public decimal ImdbRating { get; set; }

    public int DurationMinutes { get; set; }

    public long ViewCount { get; set; }

    public int EpisodeCount { get; set; }

    public bool IsActive { get; set; }

    public bool IsArchived { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? ArchivedAt { get; set; }
}


// ==================================================
// TVShow Create
// ==================================================

public class TVShowCreateDto
{
    [Required(ErrorMessage = "عنوان برنامه تلویزیونی الزامی است.")]
    [MaxLength(250, ErrorMessage = "عنوان نمی‌تواند بیشتر از 250 کاراکتر باشد.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "پوستر الزامی است.")]
    public IFormFile? Poster { get; set; }

    [Required(ErrorMessage = "توضیح کوتاه الزامی است.")]
    [MaxLength(1000, ErrorMessage = "توضیح کوتاه نمی‌تواند بیشتر از 1000 کاراکتر باشد.")]
    public string ShortDescription { get; set; } = string.Empty;

    [MaxLength(1000, ErrorMessage = "لینک دانلود نمی‌تواند بیشتر از 1000 کاراکتر باشد.")]
    public string? DownloadLink { get; set; }

    [Range(1, 10000, ErrorMessage = "مدت باید بین 1 تا 10000 دقیقه باشد.")]
    public int DurationMinutes { get; set; }

    [Range(1888, 2100, ErrorMessage = "سال وارد شده معتبر نیست.")]
    public int Year { get; set; }

    [Range(0, 10, ErrorMessage = "امتیاز IMDb باید بین 0 تا 10 باشد.")]
    public decimal ImdbRating { get; set; }

    [Range(0, long.MaxValue, ErrorMessage = "تعداد بازدید نمی‌تواند منفی باشد.")]
    public long ViewCount { get; set; }

    public List<int> SelectedGenreIds { get; set; } = new();

    public List<int> SelectedLanguageIds { get; set; } = new();

    public List<MovieCastMemberDto> SelectedCastMembers { get; set; } = new();

    public List<Quality> AvailableQualities { get; set; } = new();

    public List<MovieSelectionItemDto> Genres { get; set; } = new();

    public List<MovieSelectionItemDto> Languages { get; set; } = new();

    public List<MovieCastMemberSelectionDto> CastMembers { get; set; } = new();
}


// ==================================================
// TVShow Edit
// ==================================================

public class TVShowEditDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "عنوان برنامه تلویزیونی الزامی است.")]
    [MaxLength(250, ErrorMessage = "عنوان نمی‌تواند بیشتر از 250 کاراکتر باشد.")]
    public string Title { get; set; } = string.Empty;

    public IFormFile? Poster { get; set; }

    public string ExistingPosterPath { get; set; } = string.Empty;

    [Required(ErrorMessage = "توضیح کوتاه الزامی است.")]
    [MaxLength(1000, ErrorMessage = "توضیح کوتاه نمی‌تواند بیشتر از 1000 کاراکتر باشد.")]
    public string ShortDescription { get; set; } = string.Empty;

    [MaxLength(1000, ErrorMessage = "لینک دانلود نمی‌تواند بیشتر از 1000 کاراکتر باشد.")]
    public string? DownloadLink { get; set; }

    [Range(1, 10000, ErrorMessage = "مدت باید بین 1 تا 10000 دقیقه باشد.")]
    public int DurationMinutes { get; set; }

    [Range(1888, 2100, ErrorMessage = "سال وارد شده معتبر نیست.")]
    public int Year { get; set; }

    [Range(0, 10, ErrorMessage = "امتیاز IMDb باید بین 0 تا 10 باشد.")]
    public decimal ImdbRating { get; set; }

    [Range(0, long.MaxValue, ErrorMessage = "تعداد بازدید نمی‌تواند منفی باشد.")]
    public long ViewCount { get; set; }

    public List<int> SelectedGenreIds { get; set; } = new();

    public List<int> SelectedLanguageIds { get; set; } = new();

    public List<MovieCastMemberDto> SelectedCastMembers { get; set; } = new();

    public List<Quality> AvailableQualities { get; set; } = new();

    public List<MovieSelectionItemDto> Genres { get; set; } = new();

    public List<MovieSelectionItemDto> Languages { get; set; } = new();

    public List<MovieCastMemberSelectionDto> CastMembers { get; set; } = new();
}


// ==================================================
// TVShow Details
// ==================================================

public class TVShowDetailsDto
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

    public List<MovieCastMemberDetailsDto> CastMembers { get; set; } = new();

    public int EpisodeCount { get; set; }

    public bool IsActive { get; set; }

    public bool IsArchived { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? ArchivedAt { get; set; }
}