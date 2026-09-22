namespace DataBase.Entity;

public class Genre : BaseEntity
{
    // نام ژانر
    public string Name { get; set; } = string.Empty;
    // رابطه چند به چند با فیلم‌ها
    public List<Movie> Movies { get; set; } = [];
}
