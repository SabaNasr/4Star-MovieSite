using DataBase.Enum;
namespace Service.Admin.Movies.Review;

// ==================================================
// Review - List DTO
// ==================================================

public class ReviewListDto
{
    public int Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public StarRating StarRating { get; set; }

    public bool IsApproved { get; set; }

    public int MovieId { get; set; }

    public string MovieTitle { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}


// ==================================================
// Review - Details DTO
// ==================================================

public class ReviewDetailsDto
{
    public int Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public StarRating StarRating { get; set; }

    public bool IsApproved { get; set; }

    public int MovieId { get; set; }

    public string MovieTitle { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}