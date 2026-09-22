using DataBase.Context;
using Microsoft.EntityFrameworkCore;
namespace Service.Admin.TVShows.TVShowReview;

public interface ITVShowReviewService
{
    Task<List<TVShowReviewListDto>> GetAllAsync();

    Task<TVShowReviewDetailsDto?> GetDetailsAsync(
        int id);

    Task<bool> ApproveAsync(int id);

    Task<bool> RejectAsync(int id);

    Task<bool> DeleteAsync(int id);
}

public class TVShowReviewService : ITVShowReviewService
{
    private readonly MyContext _context;

    public TVShowReviewService(MyContext context)
    {
        _context = context;
    }

    public async Task<List<TVShowReviewListDto>>
        GetAllAsync()
    {
        return await _context.Reviews
            .AsNoTracking()
            .Where(x => x.Movie is DataBase.Entity.TVShow)
            .OrderBy(x => x.IsApproved)
            .ThenByDescending(x => x.CreatedAt)
            .Select(x => new TVShowReviewListDto
            {
                Id = x.Id,
                FullName = x.FullName,
                Content = x.Content,
                StarRating = x.StarRating,
                IsApproved = x.IsApproved,
                TVShowId = x.MovieId,
                TVShowTitle = x.Movie.Title,
                CreatedAt = x.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<TVShowReviewDetailsDto?>
        GetDetailsAsync(int id)
    {
        return await _context.Reviews
            .AsNoTracking()
            .Where(x =>
                x.Id == id &&
                x.Movie is DataBase.Entity.TVShow)
            .Select(x => new TVShowReviewDetailsDto
            {
                Id = x.Id,
                FullName = x.FullName,
                Content = x.Content,
                StarRating = x.StarRating,
                IsApproved = x.IsApproved,
                TVShowId = x.MovieId,
                TVShowTitle = x.Movie.Title,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            })
            .FirstOrDefaultAsync();
    }

    public async Task<bool> ApproveAsync(int id)
    {
        var review = await _context.Reviews
            .FirstOrDefaultAsync(x =>
                x.Id == id &&
                x.Movie is DataBase.Entity.TVShow);

        if (review == null)
            return false;

        review.IsApproved = true;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> RejectAsync(int id)
    {
        var review = await _context.Reviews
            .FirstOrDefaultAsync(x =>
                x.Id == id &&
                x.Movie is DataBase.Entity.TVShow);

        if (review == null)
            return false;

        review.IsApproved = false;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var review = await _context.Reviews
            .FirstOrDefaultAsync(x =>
                x.Id == id &&
                x.Movie is DataBase.Entity.TVShow);

        if (review == null)
            return false;

        _context.Reviews.Remove(review);

        await _context.SaveChangesAsync();

        return true;
    }
}