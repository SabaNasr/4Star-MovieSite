namespace Service.Admin.Blog.BlogComment;
public class BlogCommentListDto
{
    public int Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public bool IsApproved { get; set; }

    public int BlogPostId { get; set; }

    public string BlogPostTitle { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}

public class BlogCommentDetailsDto
{
    public int Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public bool IsApproved { get; set; }

    public int BlogPostId { get; set; }

    public string BlogPostTitle { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}