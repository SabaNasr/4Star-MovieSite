using System.ComponentModel.DataAnnotations;
using DataBase.Enum;
using Microsoft.AspNetCore.Http;


public class MovieSelectionItemDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public bool IsSelected { get; set; }
}


// ==================================================
// DTO - Movie CastMember
// ==================================================

public class MovieCastMemberDto
{
    public int CastMemberId { get; set; }

    public CastType CastType { get; set; }
}


public class MovieCastMemberSelectionDto
{
    public int Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string PhotoPath { get; set; } = string.Empty;

    public bool IsSelected { get; set; }

    public CastType CastType { get; set; }
}


public class MovieCastMemberDetailsDto
{
    public int Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string PhotoPath { get; set; } = string.Empty;

    public CastType CastType { get; set; }
}


// ==================================================
// DTO - Movie Create
// ==================================================

public class MovieCreateDto
{
    [Required(ErrorMessage = "عنوان فیلم الزامی است.")]
    [MaxLength(250, ErrorMessage = "عنوان فیلم نمی‌تواند بیشتر از 250 کاراکتر باشد.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "پوستر فیلم الزامی است.")]
    public IFormFile? Poster { get; set; }

    [Required(ErrorMessage = "توضیح کوتاه الزامی است.")]
    [MaxLength(1000, ErrorMessage = "توضیح کوتاه نمی‌تواند بیشتر از 1000 کاراکتر باشد.")]
    public string ShortDescription { get; set; } = string.Empty;

    // لینک دانلود - اختیاری
    [MaxLength(1000, ErrorMessage = "لینک دانلود نمی‌تواند بیشتر از 1000 کاراکتر باشد.")]
    public string? DownloadLink { get; set; }

    [Required(ErrorMessage = "مدت زمان فیلم الزامی است.")]
    [Range(1, 10000, ErrorMessage = "مدت زمان فیلم نامعتبر است.")]
    public int DurationMinutes { get; set; }

    [Required(ErrorMessage = "سال ساخت فیلم الزامی است.")]
    [Range(1888, 2100, ErrorMessage = "سال ساخت فیلم نامعتبر است.")]
    public int Year { get; set; }

    [Required(ErrorMessage = "امتیاز IMDb الزامی است.")]
    [Range(0, 10, ErrorMessage = "امتیاز IMDb باید بین 0 تا 10 باشد.")]
    public decimal ImdbRating { get; set; }

    [Range(0, long.MaxValue, ErrorMessage = "تعداد بازدید نمی‌تواند منفی باشد.")]
    public long ViewCount { get; set; }

    // ژانرهای انتخاب شده
    public List<int> SelectedGenreIds { get; set; } = [];

    // زبان‌های انتخاب شده
    public List<int> SelectedLanguageIds { get; set; } = [];

    // کیفیت‌های فیلم
    public List<Quality> AvailableQualities { get; set; } = [];

    // عوامل انتخاب شده
    public List<MovieCastMemberDto> SelectedCastMembers { get; set; } = [];

    // لیست ژانرها برای نمایش فرم
    public List<MovieSelectionItemDto> Genres { get; set; } = [];

    // لیست زبان‌ها برای نمایش فرم
    public List<MovieSelectionItemDto> Languages { get; set; } = [];

    // لیست عوامل برای نمایش فرم
    public List<MovieCastMemberSelectionDto> CastMembers { get; set; } = [];
}


// ==================================================
// DTO - Movie Edit
// ==================================================

public class MovieEditDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "عنوان فیلم الزامی است.")]
    [MaxLength(250, ErrorMessage = "عنوان فیلم نمی‌تواند بیشتر از 250 کاراکتر باشد.")]
    public string Title { get; set; } = string.Empty;

    public IFormFile? Poster { get; set; }

    public string ExistingPosterPath { get; set; } = string.Empty;

    [Required(ErrorMessage = "توضیح کوتاه الزامی است.")]
    [MaxLength(1000, ErrorMessage = "توضیح کوتاه نمی‌تواند بیشتر از 1000 کاراکتر باشد.")]
    public string ShortDescription { get; set; } = string.Empty;

    // لینک دانلود - اختیاری
    [MaxLength(1000, ErrorMessage = "لینک دانلود نمی‌تواند بیشتر از 1000 کاراکتر باشد.")]
    public string? DownloadLink { get; set; }

    [Required(ErrorMessage = "مدت زمان فیلم الزامی است.")]
    [Range(1, 10000, ErrorMessage = "مدت زمان فیلم نامعتبر است.")]
    public int DurationMinutes { get; set; }

    [Required(ErrorMessage = "سال ساخت فیلم الزامی است.")]
    [Range(1888, 2100, ErrorMessage = "سال ساخت فیلم نامعتبر است.")]
    public int Year { get; set; }

    [Required(ErrorMessage = "امتیاز IMDb الزامی است.")]
    [Range(0, 10, ErrorMessage = "امتیاز IMDb باید بین 0 تا 10 باشد.")]
    public decimal ImdbRating { get; set; }

    [Range(0, long.MaxValue, ErrorMessage = "تعداد بازدید نمی‌تواند منفی باشد.")]
    public long ViewCount { get; set; }

    // ژانرهای انتخاب شده
    public List<int> SelectedGenreIds { get; set; } = [];

    // زبان‌های انتخاب شده
    public List<int> SelectedLanguageIds { get; set; } = [];

    // کیفیت‌های فیلم
    public List<Quality> AvailableQualities { get; set; } = [];

    // عوامل انتخاب شده
    public List<MovieCastMemberDto> SelectedCastMembers { get; set; } = [];

    // لیست ژانرها برای نمایش فرم
    public List<MovieSelectionItemDto> Genres { get; set; } = [];

    // لیست زبان‌ها برای نمایش فرم
    public List<MovieSelectionItemDto> Languages { get; set; } = [];

    // لیست عوامل برای نمایش فرم
    public List<MovieCastMemberSelectionDto> CastMembers { get; set; } = [];
}


// ==================================================
// DTO - Movie List
// ==================================================

public class MovieListDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string PosterPath { get; set; } = string.Empty;

    public int Year { get; set; }

    public decimal ImdbRating { get; set; }

    public int DurationMinutes { get; set; }

    public long ViewCount { get; set; }

    public bool IsActive { get; set; }

    public bool IsArchived { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? ArchivedAt { get; set; }
}


// ==================================================
// DTO - Movie Details
// ==================================================

public class MovieDetailsDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string PosterPath { get; set; } = string.Empty;

    public string ShortDescription { get; set; } = string.Empty;

    // لینک دانلود - اختیاری
    public string? DownloadLink { get; set; }

    public int DurationMinutes { get; set; }

    public int Year { get; set; }

    public decimal ImdbRating { get; set; }

    public long ViewCount { get; set; }

    public List<string> Genres { get; set; } = [];

    public List<string> Languages { get; set; } = [];

    public List<Quality> AvailableQualities { get; set; } = [];

    public List<MovieCastMemberDetailsDto> CastMembers { get; set; } = [];

    public bool IsActive { get; set; }

    public bool IsArchived { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}