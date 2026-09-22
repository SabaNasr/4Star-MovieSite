namespace DataBase.Entity;
public class BlogPost : BaseEntity
{
    public string Title { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    public string ImagePath { get; set; } = string.Empty;

    public string ShortDescription { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public DateTime PublishDate { get; set; }

    public int LikeCount { get; set; }

    public List<BlogCategory> Categories { get; set; } = new();

    public List<BlogComment> Comments { get; set; } = new();
}