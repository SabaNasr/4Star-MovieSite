using DataBase.Enum;
namespace DataBase.Entity;
public class Review : BaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public StarRating StarRating { get; set; }

    // آیا نظر توسط ادمین تایید شده؟
    public bool IsApproved { get; set; }
    public int MovieId { get; set; }
    public Movie Movie { get; set; } = null!;
}
