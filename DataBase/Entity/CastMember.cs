namespace DataBase.Entity;

public class CastMember : BaseEntity
{
    public string FullName { get; set; } = string.Empty;

    // عکس شخص
    public string PhotoPath { get; set; } = string.Empty;

    // رابطه چند به چند با فیلم‌ها
    public List<Movie> Movies { get; set; } = [];
}