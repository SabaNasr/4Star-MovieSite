using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
namespace Service.Admin.Content.AboutUsTeamMember;

public class AboutUsTeamMemberListDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string JobTitle { get; set; } = string.Empty;
    public string ImagePath { get; set; } = string.Empty;
    public int Order { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class AboutUsTeamMemberCreateDto
{
    [Required(ErrorMessage = "نام عضو تیم الزامی است.")]
    [MaxLength(150)]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "عنوان شغلی الزامی است.")]
    [MaxLength(150)]
    public string JobTitle { get; set; } = string.Empty;

    [Required(ErrorMessage = "تصویر عضو تیم الزامی است.")]
    public IFormFile? Image { get; set; }

    [Range(0, int.MaxValue,
        ErrorMessage = "ترتیب نمایش نمی‌تواند منفی باشد.")]
    public int Order { get; set; }

    public bool IsActive { get; set; } = true;

    [Required(ErrorMessage = "انتخاب صفحه درباره ما الزامی است.")]
    public int AboutUsId { get; set; }
}

public class AboutUsTeamMemberEditDto
{
    public int Id { get; set; }
    public string ExistingImagePath { get; set; } =
        string.Empty;

    [Required(ErrorMessage = "نام عضو تیم الزامی است.")]
    [MaxLength(150)]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "عنوان شغلی الزامی است.")]
    [MaxLength(150)]
    public string JobTitle { get; set; } = string.Empty;

    public IFormFile? Image { get; set; }

    [Range(0, int.MaxValue,
        ErrorMessage = "ترتیب نمایش نمی‌تواند منفی باشد.")]
    public int Order { get; set; }

    public bool IsActive { get; set; }

    public int AboutUsId { get; set; }
}

public class AboutUsTeamMemberDetailsDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string JobTitle { get; set; } = string.Empty;
    public string ImagePath { get; set; } = string.Empty;
    public int Order { get; set; }
    public bool IsActive { get; set; }
    public int AboutUsId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}