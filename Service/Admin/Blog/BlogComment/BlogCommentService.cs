using DataBase.Context;
using Microsoft.EntityFrameworkCore;
namespace Service.Admin.Blog.BlogComment;

// ==================================================
// Interface 
// ==================================================
public interface IBlogCommentService
{
    Task<List<BlogCommentListDto>> GetAllAsync();

    Task<BlogCommentDetailsDto?> GetDetailsAsync(int id);

    Task<bool> ApproveAsync(int id);

    Task<bool> RejectAsync(int id);

    Task<bool> DeleteAsync(int id);
}
// ==================================================
// Service 
// ==================================================
public class BlogCommentService : IBlogCommentService
{
    private readonly MyContext _context;

    public BlogCommentService(MyContext context)
    {
        _context = context;
    }

    public async Task<List<BlogCommentListDto>> GetAllAsync()
    {
        return await _context.BlogComments
            .AsNoTracking()
            .OrderBy(x => x.IsApproved)
            .ThenByDescending(x => x.CreatedAt)
            .Select(x => new BlogCommentListDto
            {
                Id = x.Id,
                FullName = x.FullName,
                Content = x.Content,
                IsApproved = x.IsApproved,
                BlogPostId = x.BlogPostId,
                BlogPostTitle = x.BlogPost.Title,
                CreatedAt = x.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<BlogCommentDetailsDto?> GetDetailsAsync(int id)
    {
        return await _context.BlogComments
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new BlogCommentDetailsDto
            {
                Id = x.Id,
                FullName = x.FullName,
                Content = x.Content,
                IsApproved = x.IsApproved,
                BlogPostId = x.BlogPostId,
                BlogPostTitle = x.BlogPost.Title,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            })
            .FirstOrDefaultAsync();
    }

    public async Task<bool> ApproveAsync(int id)
    {
        var comment = await _context.BlogComments
            .FirstOrDefaultAsync(x => x.Id == id);

        if (comment == null)
            return false;

        comment.IsApproved = true;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> RejectAsync(int id)
    {
        var comment = await _context.BlogComments
            .FirstOrDefaultAsync(x => x.Id == id);

        if (comment == null)
            return false;

        comment.IsApproved = false;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var comment = await _context.BlogComments
            .FirstOrDefaultAsync(x => x.Id == id);

        if (comment == null)
            return false;

        _context.BlogComments.Remove(comment);

        await _context.SaveChangesAsync();

        return true;
    }
}