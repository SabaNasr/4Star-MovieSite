using DataBase.Enum;
namespace Service.Admin.Content.AboutUsComment;

public class AboutUsCommentListDto
{
    public int Id { get; set; }

    public string FullName { get; set; } =
        string.Empty;

    public string JobTitle { get; set; } =
        string.Empty;

    public string Content { get; set; } =
        string.Empty;

    public StarRating StarRating { get; set; }

    public bool IsApproved { get; set; }

    public int AboutUsId { get; set; }

    public DateTime CreatedAt { get; set; }
}

public class AboutUsCommentDetailsDto
{
    public int Id { get; set; }

    public string FullName { get; set; } =
        string.Empty;

    public string JobTitle { get; set; } =
        string.Empty;

    public string Content { get; set; } =
        string.Empty;

    public StarRating StarRating { get; set; }

    public bool IsApproved { get; set; }

    public int AboutUsId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}