using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
namespace Service.Admin.Content.Banner;

public class BannerCreateDto
{
    [Required(ErrorMessage = "تصویر بنر الزامی است.")]
    [Display(Name = "تصویر بنر")]
    public IFormFile Image { get; set; } = null!;

    [Display(Name = "متن جایگزین (Alt)")]
    [MaxLength(250)]
    public string? Alt { get; set; }

    [Display(Name = "متن روی بنر")]
    [MaxLength(250)]
    public string? Text { get; set; }

    [Display(Name = "لینک مقصد")]
    [MaxLength(500)]
    public string? Link { get; set; }

    [Display(Name = "تگ جستجوی مرتبط (اختیاری)")]
    public int? TagSearchId { get; set; }

    [Range(0, 9999)]
    [Display(Name = "ترتیب نمایش")]
    public int Order { get; set; } = 0;

    [Display(Name = "فعال")]
    public bool IsActive { get; set; } = true;
}

public class BannerUpdateDto : BannerCreateDto
{
    [Required]
    public int Id { get; set; }

    [Display(Name = "تصویر جدید (اختیاری)")]
    public IFormFile? NewImage { get; set; }

    [Display(Name = "تصویر فعلی")]
    public string? ExistingImage { get; set; }
}

public class BannerListDto
{
    public int Id { get; set; }
    public string Image { get; set; } = string.Empty;
    public string? Text { get; set; }
    public string? TagName { get; set; } // نام تگ مرتبط برای نمایش در لیست
    public int Order { get; set; }
    public bool IsActive { get; set; }
}