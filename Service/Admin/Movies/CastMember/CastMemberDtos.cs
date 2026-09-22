using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
namespace Service.Admin.Movies.CastMember;

public class CastMemberCreateDto
{
    [Required(ErrorMessage = "نام شخص الزامی است.")]
    [MaxLength(200, ErrorMessage = "نام شخص نمی‌تواند بیشتر از 200 کاراکتر باشد.")]
    public string FullName { get; set; } = string.Empty;

    public IFormFile? Photo { get; set; }
}

public class CastMemberEditDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "نام شخص الزامی است.")]
    [MaxLength(200, ErrorMessage = "نام شخص نمی‌تواند بیشتر از 200 کاراکتر باشد.")]
    public string FullName { get; set; } = string.Empty;

    public IFormFile? Photo { get; set; }

    public string ExistingPhotoPath { get; set; } = string.Empty;
}

public class CastMemberListDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string PhotoPath { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CastMemberMovieDto
{
    public int MovieId { get; set; }
    public string MovieTitle { get; set; } = string.Empty;
    public DataBase.Enum.CastType CastType { get; set; }
}

public class CastMemberDetailsDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string PhotoPath { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public List<CastMemberMovieDto> Movies { get; set; } = [];
}