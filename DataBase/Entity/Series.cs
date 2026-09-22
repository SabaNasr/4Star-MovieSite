namespace DataBase.Entity;

public class Series : Movie
{
    public List<Season> Seasons { get; set; } = new();
}
