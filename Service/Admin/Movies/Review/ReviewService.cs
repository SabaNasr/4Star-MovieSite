using DataBase.Context;
using Microsoft.EntityFrameworkCore;
namespace Service.Admin.Movies.Review;

// ==================================================
// Interface - Review
// ==================================================

public interface IReviewService
{
    Task<List<ReviewListDto>> GetAllAsync();

    Task<ReviewDetailsDto?> GetDetailsAsync(int id);

    Task<bool> ApproveAsync(int id);

    Task<bool> RejectAsync(int id);

    Task<bool> DeleteAsync(int id);
}


// ==================================================
// Service - Review
// ==================================================

public class ReviewService : IReviewService
{
    private readonly MyContext _context;


    public ReviewService(MyContext context)
    {
        _context = context;
    }


    // ==================================================
    // دریافت لیست تمام نظرات
    // ==================================================

    public async Task<List<ReviewListDto>> GetAllAsync()
    {
        return await _context.Reviews
            .AsNoTracking()
            .OrderBy(x => x.IsApproved)
            .ThenByDescending(x => x.CreatedAt)
            .Select(x => new ReviewListDto
            {
                Id = x.Id,

                FullName = x.FullName,

                Content = x.Content,

                StarRating = x.StarRating,

                IsApproved = x.IsApproved,

                MovieId = x.MovieId,

                MovieTitle = x.Movie.Title,

                CreatedAt = x.CreatedAt
            })
            .ToListAsync();
    }


    // ==================================================
    // دریافت جزئیات نظر
    // ==================================================

    public async Task<ReviewDetailsDto?> GetDetailsAsync(int id)
    {
        return await _context.Reviews
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new ReviewDetailsDto
            {
                Id = x.Id,

                FullName = x.FullName,

                Content = x.Content,

                StarRating = x.StarRating,

                IsApproved = x.IsApproved,

                MovieId = x.MovieId,

                MovieTitle = x.Movie.Title,

                CreatedAt = x.CreatedAt,

                UpdatedAt = x.UpdatedAt
            })
            .FirstOrDefaultAsync();
    }


    // ==================================================
    // تایید نظر
    // ==================================================

    public async Task<bool> ApproveAsync(int id)
    {
        var review = await _context.Reviews
            .FirstOrDefaultAsync(x => x.Id == id);


        if (review == null)
            return false;


        review.IsApproved = true;


        await _context.SaveChangesAsync();

        return true;
    }


    // ==================================================
    // عدم تایید نظر
    // ==================================================

    public async Task<bool> RejectAsync(int id)
    {
        var review = await _context.Reviews
            .FirstOrDefaultAsync(x => x.Id == id);


        if (review == null)
            return false;


        review.IsApproved = false;


        await _context.SaveChangesAsync();

        return true;
    }


    // ==================================================
    // حذف نرم نظر
    // ==================================================

    public async Task<bool> DeleteAsync(int id)
    {
        var review = await _context.Reviews
            .FirstOrDefaultAsync(x => x.Id == id);


        if (review == null)
            return false;


        _context.Reviews.Remove(review);


        await _context.SaveChangesAsync();

        return true;
    }
}