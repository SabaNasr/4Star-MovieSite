using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
namespace Service.Admin.Content.AboutUs;

public class AboutUsListDto
{
    public int Id { get; set; }
    public string ImagePath { get; set; } = string.Empty;
    public int CustomerCount { get; set; }
    public int ActiveUserCount { get; set; }
    public int TotalVideoCount { get; set; }
    public int SubscriberCount { get; set; }
    public int AwardCount { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class AboutUsCreateDto
{
    [Required(ErrorMessage = "تصویر درباره ما الزامی است.")]
    public IFormFile? Image { get; set; }

    [Required(ErrorMessage = "محتوای درباره ما الزامی است.")]
    public string Content { get; set; } = string.Empty;

    [Range(0, int.MaxValue,
        ErrorMessage = "تعداد مشتریان نمی‌تواند منفی باشد.")]
    public int CustomerCount { get; set; }

    [Range(0, int.MaxValue,
        ErrorMessage = "تعداد کاربران فعال نمی‌تواند منفی باشد.")]
    public int ActiveUserCount { get; set; }

    [Range(0, int.MaxValue,
        ErrorMessage = "تعداد کل ویدیوها نمی‌تواند منفی باشد.")]
    public int TotalVideoCount { get; set; }

    [Range(0, int.MaxValue,
        ErrorMessage = "تعداد مشترکین نمی‌تواند منفی باشد.")]
    public int SubscriberCount { get; set; }

    [Range(0, int.MaxValue,
        ErrorMessage = "تعداد جوایز نمی‌تواند منفی باشد.")]
    public int AwardCount { get; set; }

    public bool IsActive { get; set; } = true;
}

public class AboutUsEditDto
{
    public int Id { get; set; }

    public string ExistingImagePath { get; set; } =
        string.Empty;

    public IFormFile? Image { get; set; }

    [Required(ErrorMessage = "محتوای درباره ما الزامی است.")]
    public string Content { get; set; } = string.Empty;

    [Range(0, int.MaxValue,
        ErrorMessage = "تعداد مشتریان نمی‌تواند منفی باشد.")]
    public int CustomerCount { get; set; }

    [Range(0, int.MaxValue,
        ErrorMessage = "تعداد کاربران فعال نمی‌تواند منفی باشد.")]
    public int ActiveUserCount { get; set; }

    [Range(0, int.MaxValue,
        ErrorMessage = "تعداد کل ویدیوها نمی‌تواند منفی باشد.")]
    public int TotalVideoCount { get; set; }

    [Range(0, int.MaxValue,
        ErrorMessage = "تعداد مشترکین نمی‌تواند منفی باشد.")]
    public int SubscriberCount { get; set; }

    [Range(0, int.MaxValue,
        ErrorMessage = "تعداد جوایز نمی‌تواند منفی باشد.")]
    public int AwardCount { get; set; }

    public bool IsActive { get; set; }
}

public class AboutUsDetailsDto
{
    public int Id { get; set; }
    public string ImagePath { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;

    public int CustomerCount { get; set; }
    public int ActiveUserCount { get; set; }

    public int TotalVideoCount { get; set; }
    public int SubscriberCount { get; set; }
    public int AwardCount { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}