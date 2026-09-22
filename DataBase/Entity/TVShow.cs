namespace DataBase.Entity;

public class TVShow : Movie
{
    public List<TVShowEpisode> Episodes { get; set; } = new();
}
