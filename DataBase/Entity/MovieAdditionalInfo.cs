namespace DataBase.Entity;

public class MovieAdditionalInfo : BaseEntity
{
    // متن اطلاعات تکمیلی
    public string Content { get; set; } = string.Empty;
    public int MovieId { get; set; }
    public Movie Movie { get; set; } = null!;
}