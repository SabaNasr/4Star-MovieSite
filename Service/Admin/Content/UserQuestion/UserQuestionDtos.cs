using System.ComponentModel.DataAnnotations;
namespace Service.Admin.Content.UserQuestion;

public class UserQuestionListDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class UserQuestionDetailsDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;

    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}