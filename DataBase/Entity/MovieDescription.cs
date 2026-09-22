namespace DataBase.Entity;

public class MovieDescription : BaseEntity
{
    // متن کامل بخش شرح
    public string Content { get; set; } = string.Empty;
    public int MovieId { get; set; }
    public Movie Movie { get; set; } = null!;
}