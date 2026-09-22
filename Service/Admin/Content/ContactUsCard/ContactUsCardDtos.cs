using System.ComponentModel.DataAnnotations;
namespace Service.Admin.Content.ContactUsCard;

public class ContactUsCardListDto
{
    public int Id { get; set; }

    public string Icon { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public int DisplayOrder { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}

public class ContactUsCardCreateDto
{
    [Required(ErrorMessage = "آیکون الزامی است.")]
    [MaxLength(
        100,
        ErrorMessage = "آیکون نمی‌تواند بیشتر از 100 کاراکتر باشد.")]
    public string Icon { get; set; } = string.Empty;

    [Required(ErrorMessage = "عنوان کارت الزامی است.")]
    [MaxLength(
        150,
        ErrorMessage = "عنوان کارت نمی‌تواند بیشتر از 150 کاراکتر باشد.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "محتوای کارت الزامی است.")]
    public string Content { get; set; } = string.Empty;

    [Range(
        0,
        int.MaxValue,
        ErrorMessage = "ترتیب نمایش نمی‌تواند منفی باشد.")]
    public int DisplayOrder { get; set; }

    public bool IsActive { get; set; } = true;
}

public class ContactUsCardEditDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "آیکون الزامی است.")]
    [MaxLength(
        100,
        ErrorMessage = "آیکون نمی‌تواند بیشتر از 100 کاراکتر باشد.")]
    public string Icon { get; set; } = string.Empty;

    [Required(ErrorMessage = "عنوان کارت الزامی است.")]
    [MaxLength(
        150,
        ErrorMessage = "عنوان کارت نمی‌تواند بیشتر از 150 کاراکتر باشد.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "محتوای کارت الزامی است.")]
    public string Content { get; set; } = string.Empty;

    [Range(
        0,
        int.MaxValue,
        ErrorMessage = "ترتیب نمایش نمی‌تواند منفی باشد.")]
    public int DisplayOrder { get; set; }

    public bool IsActive { get; set; }
}

public class ContactUsCardDetailsDto
{
    public int Id { get; set; }

    public string Icon { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public int DisplayOrder { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}