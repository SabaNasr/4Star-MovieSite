namespace DataBase.Entity;
public class Season : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string? DownloadLink { get; set; }

    public int SeriesId { get; set; }

    public Series Series { get; set; } = null!;

    public List<Episode> Episodes { get; set; } = new();
}