using DataBase.Enum;
namespace Service.Admin.TVShows.TVShowReview;

public class TVShowReviewListDto
{
    public int Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public StarRating StarRating { get; set; }

    public bool IsApproved { get; set; }

    public int TVShowId { get; set; }

    public string TVShowTitle { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}

public class TVShowReviewDetailsDto
{
    public int Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public StarRating StarRating { get; set; }

    public bool IsApproved { get; set; }

    public int TVShowId { get; set; }

    public string TVShowTitle { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}