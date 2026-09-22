namespace DataBase.Entity;

public class Language : BaseEntity
{
    // نام زبان
    public string Name { get; set; } = string.Empty;

    // رابطه چند به چند با فیلم‌ها
    public List<Movie> Movies { get; set; } = [];
}