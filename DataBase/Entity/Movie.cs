using DataBase.Enum;
namespace DataBase.Entity;

public class Movie : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string PosterPath { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string? DownloadLink { get; set; }
    public int DurationMinutes { get; set; }

    // سال ساخت فیلم
    public int Year { get; set; }

    // امتیاز IMDb
    public decimal ImdbRating { get; set; }

    // تعداد بازدید فیلم
    public long ViewCount { get; set; }

    // کیفیت‌های موجود برای فیلم
    public List<Quality> AvailableQualities { get; set; } = [];

    // رابطه چند به چند با Genre
    public List<Genre> Genres { get; set; } = [];

    // رابطه چند به چند با Language
    public List<Language> Languages { get; set; } = [];

    // رابطه چند به چند با عوامل فیلم
    public List<CastMember> CastMembers { get; set; } = [];

    // رابطه یک به چند با نظرات کاربران
    public List<Review> Reviews { get; set; } = [];

    // رابطه یک به یک با بخش شرح
    public MovieDescription? Description { get; set; }

    // رابطه یک به یک با اطلاعات تکمیلی
    public MovieAdditionalInfo? AdditionalInfo { get; set; }
}