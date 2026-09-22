using DataBase.Enum;
namespace Service.Admin.TvSeries.SeriesReview;

public class SeriesReviewListDto
{
    public int Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public StarRating StarRating { get; set; }

    public bool IsApproved { get; set; }

    public int SeriesId { get; set; }

    public string SeriesTitle { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}

public class SeriesReviewDetailsDto
{
    public int Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public StarRating StarRating { get; set; }

    public bool IsApproved { get; set; }

    public int SeriesId { get; set; }

    public string SeriesTitle { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}