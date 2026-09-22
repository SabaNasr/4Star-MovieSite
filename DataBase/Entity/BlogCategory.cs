namespace DataBase.Entity;
public class BlogCategory : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int DisplayOrder { get; set; }

    public List<BlogPost> BlogPosts { get; set; } = new();
}
