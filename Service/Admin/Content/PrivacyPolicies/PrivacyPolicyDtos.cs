using System.ComponentModel.DataAnnotations;
namespace Service.Admin.Content.PrivacyPolicies;

public class PrivacyPolicyListDto
{
    public int Id { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}

public class PrivacyPolicyCreateDto
{
    [Required(ErrorMessage = "متن سیاست حفظ حریم خصوصی الزامی است.")]
    public string Content { get; set; } = string.Empty;
}

public class PrivacyPolicyEditDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "متن سیاست حفظ حریم خصوصی الزامی است.")]
    public string Content { get; set; } = string.Empty;
}

public class PrivacyPolicyDetailsDto
{
    public int Id { get; set; }

    public string Content { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}