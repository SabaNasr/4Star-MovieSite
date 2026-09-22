using System.ComponentModel.DataAnnotations;
namespace Service.Admin.Movies.Genre;

public class GenreCreateDto
{
    [Required(ErrorMessage = "نام ژانر الزامی است.")]
    [MaxLength(100, ErrorMessage = "نام ژانر نمی‌تواند بیشتر از 100 کاراکتر باشد.")]
    public string Name { get; set; } = string.Empty;
}

public class GenreEditDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "نام ژانر الزامی است.")]
    [MaxLength(100, ErrorMessage = "نام ژانر نمی‌تواند بیشتر از 100 کاراکتر باشد.")]
    public string Name { get; set; } = string.Empty;
}

public class GenreListDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}