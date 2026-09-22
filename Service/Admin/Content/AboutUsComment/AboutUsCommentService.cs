using DataBase.Context;
using Microsoft.EntityFrameworkCore;
namespace Service.Admin.Content.AboutUsComment;

public interface IAboutUsCommentService
{
    Task<List<AboutUsCommentListDto>> GetAllAsync();

    Task<AboutUsCommentDetailsDto?> GetDetailsAsync(
        int id);

    Task<bool> ApproveAsync(int id);

    Task<bool> RejectAsync(int id);

    Task<bool> DeleteAsync(int id);
}

public class AboutUsCommentService
    : IAboutUsCommentService
{
    private readonly MyContext _context;

    public AboutUsCommentService(MyContext context)
    {
        _context = context;
    }

    public async Task<List<AboutUsCommentListDto>>
        GetAllAsync()
    {
        return await _context.AboutUsComments
            .AsNoTracking()
            .OrderBy(x => x.IsApproved)
            .ThenByDescending(x => x.CreatedAt)
            .Select(x => new AboutUsCommentListDto
            {
                Id = x.Id,
                FullName = x.FullName,
                JobTitle = x.JobTitle,
                Content = x.Content,
                StarRating = x.StarRating,
                IsApproved = x.IsApproved,
                AboutUsId = x.AboutUsId,
                CreatedAt = x.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<AboutUsCommentDetailsDto?>
        GetDetailsAsync(int id)
    {
        return await _context.AboutUsComments
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new AboutUsCommentDetailsDto
            {
                Id = x.Id,
                FullName = x.FullName,
                JobTitle = x.JobTitle,
                Content = x.Content,
                StarRating = x.StarRating,
                IsApproved = x.IsApproved,
                AboutUsId = x.AboutUsId,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            })
            .FirstOrDefaultAsync();
    }

    public async Task<bool> ApproveAsync(int id)
    {
        var comment =
            await _context.AboutUsComments
                .FirstOrDefaultAsync(x => x.Id == id);

        if (comment == null)
            return false;

        comment.IsApproved = true;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> RejectAsync(int id)
    {
        var comment =
            await _context.AboutUsComments
                .FirstOrDefaultAsync(x => x.Id == id);

        if (comment == null)
            return false;

        comment.IsApproved = false;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var comment =
            await _context.AboutUsComments
                .FirstOrDefaultAsync(x => x.Id == id);

        if (comment == null)
            return false;

        _context.AboutUsComments.Remove(comment);

        await _context.SaveChangesAsync();

        return true;
    }
}