namespace DataBase.Entity;

public class BlogComment : BaseEntity
{
    public string FullName { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public bool IsApproved { get; set; }

    public int BlogPostId { get; set; }

    public BlogPost BlogPost { get; set; } = null!;
}
