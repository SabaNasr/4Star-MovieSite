using System.ComponentModel.DataAnnotations;
namespace Service.Admin.Content.AboutUsSocialLink;

public class AboutUsSocialLinkListDto
{
    public int Id { get; set; }
    public string Icon { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public int Order { get; set; }
    public bool IsActive { get; set; }
    public int TeamMemberId { get; set; }
    public string TeamMemberName { get; set; } =
        string.Empty;

    public DateTime CreatedAt { get; set; }
}

public class AboutUsSocialLinkCreateDto
{
    [Required(ErrorMessage = "انتخاب عضو تیم الزامی است.")]
    public int TeamMemberId { get; set; }

    [Required(ErrorMessage = "آیکون الزامی است.")]
    [MaxLength(100)]
    public string Icon { get; set; } = string.Empty;

    [Required(ErrorMessage = "آدرس شبکه اجتماعی الزامی است.")]
    [MaxLength(1000)]
    [Url(ErrorMessage = "آدرس شبکه اجتماعی معتبر نیست.")]
    public string Url { get; set; } = string.Empty;

    [Range(0, int.MaxValue,
        ErrorMessage = "ترتیب نمایش نمی‌تواند منفی باشد.")]
    public int Order { get; set; }

    public bool IsActive { get; set; } = true;

    public List<
        AboutUsSocialLinkTeamMemberItemDto>
        TeamMembers
    { get; set; } = new();
}

public class AboutUsSocialLinkEditDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "انتخاب عضو تیم الزامی است.")]
    public int TeamMemberId { get; set; }

    [Required(ErrorMessage = "آیکون الزامی است.")]
    [MaxLength(100)]
    public string Icon { get; set; } = string.Empty;

    [Required(ErrorMessage = "آدرس شبکه اجتماعی الزامی است.")]
    [MaxLength(1000)]
    [Url(ErrorMessage = "آدرس شبکه اجتماعی معتبر نیست.")]
    public string Url { get; set; } = string.Empty;

    [Range(0, int.MaxValue,
        ErrorMessage = "ترتیب نمایش نمی‌تواند منفی باشد.")]
    public int Order { get; set; }

    public bool IsActive { get; set; }
}

public class AboutUsSocialLinkDetailsDto
{
    public int Id { get; set; }
    public string Icon { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public int Order { get; set; }
    public bool IsActive { get; set; }

    public int TeamMemberId { get; set; }
    public string TeamMemberName { get; set; } =
        string.Empty;

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class AboutUsSocialLinkTeamMemberItemDto
{
    public int Id { get; set; }
    public string FullName { get; set; } =
        string.Empty;
}