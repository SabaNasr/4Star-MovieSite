using System.ComponentModel.DataAnnotations;
namespace Service.Admin.Content.TermsAndConditions;

public class TermsAndConditionsListDto
{
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}

public class TermsAndConditionsCreateDto
{
    [Required(ErrorMessage = "محتوای شرایط و ضوابط الزامی است.")]
    public string Content { get; set; } = string.Empty;
}

public class TermsAndConditionsEditDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "محتوای شرایط و ضوابط الزامی است.")]
    public string Content { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}

public class TermsAndConditionsDetailsDto
{
    public int Id { get; set; }

    public string Content { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}