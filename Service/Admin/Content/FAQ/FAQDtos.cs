using System.ComponentModel.DataAnnotations;
namespace Service.Admin.Content.FAQ;

public class FAQListDto
{
    public int Id { get; set; }
    public string Question { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
    public int Order { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class FAQCreateDto
{
    [Required(ErrorMessage = "سوال الزامی است.")]
    [MaxLength(
        500,
        ErrorMessage = "سوال نمی‌تواند بیشتر از 500 کاراکتر باشد.")]
    public string Question { get; set; } = string.Empty;

    [Required(ErrorMessage = "پاسخ الزامی است.")]
    public string Answer { get; set; } = string.Empty;

    [Range(
        0,
        int.MaxValue,
        ErrorMessage = "ترتیب نمایش نمی‌تواند منفی باشد.")]
    public int Order { get; set; }

    public bool IsActive { get; set; } = true;
}

public class FAQEditDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "سوال الزامی است.")]
    [MaxLength(
        500,
        ErrorMessage = "سوال نمی‌تواند بیشتر از 500 کاراکتر باشد.")]
    public string Question { get; set; } = string.Empty;

    [Required(ErrorMessage = "پاسخ الزامی است.")]
    public string Answer { get; set; } = string.Empty;

    [Range(
        0,
        int.MaxValue,
        ErrorMessage = "ترتیب نمایش نمی‌تواند منفی باشد.")]
    public int Order { get; set; }

    public bool IsActive { get; set; }
}

public class FAQDetailsDto
{
    public int Id { get; set; }
    public string Question { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
    public int Order { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}