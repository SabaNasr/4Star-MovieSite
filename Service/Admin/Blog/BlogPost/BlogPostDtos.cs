using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
namespace Service.Admin.Blog.BlogPost;

public class BlogPostListDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    public string ImagePath { get; set; } = string.Empty;

    public DateTime PublishDate { get; set; }

    public int LikeCount { get; set; }

    public bool IsActive { get; set; }

    public bool IsArchived { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}

public class BlogPostCreateDto
{
    [Required(ErrorMessage = "عنوان پست الزامی است.")]
    [MaxLength(250, ErrorMessage = "عنوان پست نمی‌تواند بیشتر از 250 کاراکتر باشد.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Slug الزامی است.")]
    [MaxLength(250, ErrorMessage = "Slug نمی‌تواند بیشتر از 250 کاراکتر باشد.")]
    public string Slug { get; set; } = string.Empty;

    [Required(ErrorMessage = "تصویر پست الزامی است.")]
    public IFormFile? Image { get; set; }

    [Required(ErrorMessage = "توضیح کوتاه الزامی است.")]
    [MaxLength(1000, ErrorMessage = "توضیح کوتاه نمی‌تواند بیشتر از 1000 کاراکتر باشد.")]
    public string ShortDescription { get; set; } = string.Empty;

    [Required(ErrorMessage = "محتوای کامل پست الزامی است.")]
    public string Content { get; set; } = string.Empty;

    [Required(ErrorMessage = "تاریخ انتشار الزامی است.")]
    public DateTime PublishDate { get; set; } = DateTime.UtcNow;

    public bool IsActive { get; set; } = true;

    public List<int> SelectedCategoryIds { get; set; } = new();

    public List<BlogPostCategoryItemDto> Categories { get; set; } = new();
}

public class BlogPostEditDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "عنوان پست الزامی است.")]
    [MaxLength(250, ErrorMessage = "عنوان پست نمی‌تواند بیشتر از 250 کاراکتر باشد.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Slug الزامی است.")]
    [MaxLength(250, ErrorMessage = "Slug نمی‌تواند بیشتر از 250 کاراکتر باشد.")]
    public string Slug { get; set; } = string.Empty;

    public IFormFile? Image { get; set; }

    public string ExistingImagePath { get; set; } = string.Empty;

    [Required(ErrorMessage = "توضیح کوتاه الزامی است.")]
    [MaxLength(1000, ErrorMessage = "توضیح کوتاه نمی‌تواند بیشتر از 1000 کاراکتر باشد.")]
    public string ShortDescription { get; set; } = string.Empty;

    [Required(ErrorMessage = "محتوای کامل پست الزامی است.")]
    public string Content { get; set; } = string.Empty;

    [Required(ErrorMessage = "تاریخ انتشار الزامی است.")]
    public DateTime PublishDate { get; set; }

    public bool IsActive { get; set; }

    public List<int> SelectedCategoryIds { get; set; } = new();

    public List<BlogPostCategoryItemDto> Categories { get; set; } = new();
}

public class BlogPostDetailsDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    public string ImagePath { get; set; } = string.Empty;

    public string ShortDescription { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public DateTime PublishDate { get; set; }

    public int LikeCount { get; set; }

    public bool IsActive { get; set; }

    public bool IsArchived { get; set; }

    public List<string> Categories { get; set; } = new();

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}

public class BlogPostCategoryItemDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public bool IsSelected { get; set; }
}