namespace DataBase.Entity;
public class Episode : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string DownloadLink { get; set; } = string.Empty;

    public int SeasonId { get; set; }

    public Season Season { get; set; } = null!;
}