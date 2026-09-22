using System.ComponentModel.DataAnnotations;
namespace Service.Admin.Blog.BlogCategory;

public class BlogCategoryListDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int DisplayOrder { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}


public class BlogCategoryCreateDto
{
    [Required(ErrorMessage = "نام دسته‌بندی الزامی است.")]
    [MaxLength(100, ErrorMessage = "نام دسته‌بندی نمی‌تواند بیشتر از 100 کاراکتر باشد.")]
    public string Name { get; set; } = string.Empty;


    [Required(ErrorMessage = "Slug الزامی است.")]
    [MaxLength(250, ErrorMessage = "Slug نمی‌تواند بیشتر از 250 کاراکتر باشد.")]
    public string Slug { get; set; } = string.Empty;


    [MaxLength(1000, ErrorMessage = "توضیحات نمی‌تواند بیشتر از 1000 کاراکتر باشد.")]
    public string Description { get; set; } = string.Empty;


    [Range(0, int.MaxValue, ErrorMessage = "ترتیب نمایش نمی‌تواند منفی باشد.")]
    public int DisplayOrder { get; set; }


    public bool IsActive { get; set; } = true;
}


public class BlogCategoryEditDto
{
    public int Id { get; set; }


    [Required(ErrorMessage = "نام دسته‌بندی الزامی است.")]
    [MaxLength(100, ErrorMessage = "نام دسته‌بندی نمی‌تواند بیشتر از 100 کاراکتر باشد.")]
    public string Name { get; set; } = string.Empty;


    [Required(ErrorMessage = "Slug الزامی است.")]
    [MaxLength(250, ErrorMessage = "Slug نمی‌تواند بیشتر از 250 کاراکتر باشد.")]
    public string Slug { get; set; } = string.Empty;


    [MaxLength(1000, ErrorMessage = "توضیحات نمی‌تواند بیشتر از 1000 کاراکتر باشد.")]
    public string Description { get; set; } = string.Empty;


    [Range(0, int.MaxValue, ErrorMessage = "ترتیب نمایش نمی‌تواند منفی باشد.")]
    public int DisplayOrder { get; set; }


    public bool IsActive { get; set; }
}


public class BlogCategoryDetailsDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int DisplayOrder { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}