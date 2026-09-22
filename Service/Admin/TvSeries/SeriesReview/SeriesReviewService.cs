using DataBase.Context;
using Microsoft.EntityFrameworkCore;
namespace Service.Admin.TvSeries.SeriesReview;

public interface ISeriesReviewService
{
    Task<List<SeriesReviewListDto>> GetAllAsync();

    Task<SeriesReviewDetailsDto?>
        GetDetailsAsync(int id);

    Task<bool> ApproveAsync(int id);

    Task<bool> RejectAsync(int id);

    Task<bool> DeleteAsync(int id);
}

public class SeriesReviewService
    : ISeriesReviewService
{
    private readonly MyContext _context;

    public SeriesReviewService(MyContext context)
    {
        _context = context;
    }

    // ==================================================
    // Get All
    // ==================================================

    public async Task<List<SeriesReviewListDto>>
        GetAllAsync()
    {
        return await _context.Reviews
            .AsNoTracking()
            .Where(x =>
                x.Movie is DataBase.Entity.Series)
            .OrderBy(x => x.IsApproved)
            .ThenByDescending(x => x.CreatedAt)
            .Select(x => new SeriesReviewListDto
            {
                Id = x.Id,
                FullName = x.FullName,
                Content = x.Content,
                StarRating = x.StarRating,
                IsApproved = x.IsApproved,
                SeriesId = x.MovieId,
                SeriesTitle = x.Movie.Title,
                CreatedAt = x.CreatedAt
            })
            .ToListAsync();
    }

    // ==================================================
    // Get Details
    // ==================================================

    public async Task<SeriesReviewDetailsDto?>
        GetDetailsAsync(int id)
    {
        return await _context.Reviews
            .AsNoTracking()
            .Where(x =>
                x.Id == id &&
                x.Movie is DataBase.Entity.Series)
            .Select(x => new SeriesReviewDetailsDto
            {
                Id = x.Id,
                FullName = x.FullName,
                Content = x.Content,
                StarRating = x.StarRating,
                IsApproved = x.IsApproved,
                SeriesId = x.MovieId,
                SeriesTitle = x.Movie.Title,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            })
            .FirstOrDefaultAsync();
    }

    // ==================================================
    // Approve
    // ==================================================

    public async Task<bool> ApproveAsync(int id)
    {
        var review = await _context.Reviews
            .FirstOrDefaultAsync(x =>
                x.Id == id &&
                x.Movie is DataBase.Entity.Series);

        if (review == null)
            return false;

        review.IsApproved = true;

        await _context.SaveChangesAsync();

        return true;
    }

    // ==================================================
    // Reject
    // ==================================================

    public async Task<bool> RejectAsync(int id)
    {
        var review = await _context.Reviews
            .FirstOrDefaultAsync(x =>
                x.Id == id &&
                x.Movie is DataBase.Entity.Series);

        if (review == null)
            return false;

        review.IsApproved = false;

        await _context.SaveChangesAsync();

        return true;
    }

    // ==================================================
    // Delete
    // ==================================================

    public async Task<bool> DeleteAsync(int id)
    {
        var review = await _context.Reviews
            .FirstOrDefaultAsync(x =>
                x.Id == id &&
                x.Movie is DataBase.Entity.Series);

        if (review == null)
            return false;

        _context.Reviews.Remove(review);

        await _context.SaveChangesAsync();

        return true;
    }
}