using System.ComponentModel.DataAnnotations;
namespace Service.Admin.Content.TagSearch;

public class TagSearchCreateDto
{
    [Required(ErrorMessage = "عنوان تگ الزامی است.")]
    [MaxLength(150, ErrorMessage = "عنوان نمی‌تواند بیشتر از ۱۵۰ کاراکتر باشد.")]
    [Display(Name = "عنوان تگ")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "نامک الزامی است.")]
    [MaxLength(150, ErrorMessage = "نامک نمی‌تواند بیشتر از ۱۵۰ کاراکتر باشد.")]
    [RegularExpression(@"^[a-z0-9\-]+$", ErrorMessage = "فقط حروف کوچک انگلیسی، اعداد و خط تیره مجاز است.")]
    [Display(Name = "نامک (Slug)")]
    public string Slug { get; set; } = string.Empty;
}

public class TagSearchUpdateDto : TagSearchCreateDto
{
    [Required]
    public int Id { get; set; }
}

public class TagSearchListDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
}