using System.ComponentModel.DataAnnotations;
namespace Service.Admin.Movies.Language;

public class LanguageCreateDto
{
    [Required(ErrorMessage = "نام زبان الزامی است.")]
    [MaxLength(100, ErrorMessage = "نام زبان نمی‌تواند بیشتر از 100 کاراکتر باشد.")]
    public string Name { get; set; } = string.Empty;
}

public class LanguageEditDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "نام زبان الزامی است.")]
    [MaxLength(100, ErrorMessage = "نام زبان نمی‌تواند بیشتر از 100 کاراکتر باشد.")]
    public string Name { get; set; } = string.Empty;
}

public class LanguageListDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}