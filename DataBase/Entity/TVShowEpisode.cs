namespace DataBase.Entity;

public class TVShowEpisode : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string ImagePath { get; set; } = string.Empty;

    public string DownloadLink { get; set; } = string.Empty;

    public int SeasonNumber { get; set; }

    public int EpisodeNumber { get; set; }

    public int TVShowId { get; set; }

    public TVShow TVShow { get; set; } = null!;
}