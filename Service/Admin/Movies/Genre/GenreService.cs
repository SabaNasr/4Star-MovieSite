using DataBase.Context;
using Microsoft.EntityFrameworkCore;
namespace Service.Admin.Movies.Genre;

// Interface
public interface IGenreService
{
    // دریافت تمام ژانرها
    Task<List<GenreListDto>> GetAllAsync();

    // دریافت اطلاعات ژانر برای Edit
    Task<GenreEditDto?> GetEditDataAsync(int id);

    // ایجاد ژانر
    Task<bool> CreateAsync(GenreCreateDto dto);

    // ویرایش ژانر
    Task<bool> UpdateAsync(GenreEditDto dto);

    // فعال / غیرفعال کردن ژانر
    Task<bool> ChangeStatusAsync(int id);
}


// Service

public class GenreService : IGenreService
{
    private readonly MyContext _context;

    public GenreService(MyContext context)
    {
        _context = context;
    }


    // ==================================================
    // دریافت تمام ژانرها
    // ==================================================

    public async Task<List<GenreListDto>> GetAllAsync()
    {
        return await _context.Genres
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .Select(x => new GenreListDto
            {
                Id = x.Id,
                Name = x.Name,
                IsActive = x.IsActive,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            })
            .ToListAsync();
    }


    // ==================================================
    // دریافت اطلاعات ژانر برای Edit
    // ==================================================

    public async Task<GenreEditDto?> GetEditDataAsync(int id)
    {
        return await _context.Genres
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new GenreEditDto
            {
                Id = x.Id,
                Name = x.Name
            })
            .FirstOrDefaultAsync();
    }


    // ==================================================
    // ایجاد ژانر
    // ==================================================

    public async Task<bool> CreateAsync(GenreCreateDto dto)
    {
        var name = dto.Name.Trim();

        // ------------------------------------------------
        // بررسی وجود ژانر تکراری
        // ------------------------------------------------

        var exists = await _context.Genres
            .AnyAsync(x => x.Name == name);

        if (exists)
        {
            return false;
        }


        // ------------------------------------------------
        // ایجاد Genre
        // ------------------------------------------------

        var genre = new DataBase.Entity.Genre
        {
            Name = name,
            IsActive = true
        };


        _context.Genres.Add(genre);

        await _context.SaveChangesAsync();

        return true;
    }


    // ==================================================
    // ویرایش ژانر
    // ==================================================

    public async Task<bool> UpdateAsync(GenreEditDto dto)
    {
        var genre = await _context.Genres
            .FirstOrDefaultAsync(x => x.Id == dto.Id);

        if (genre == null)
        {
            return false;
        }


        var name = dto.Name.Trim();


        // ------------------------------------------------
        // بررسی تکراری نبودن نام ژانر
        // ------------------------------------------------

        var exists = await _context.Genres
            .AnyAsync(x =>
                x.Id != dto.Id &&
                x.Name == name);

        if (exists)
        {
            return false;
        }


        // ------------------------------------------------
        // بروزرسانی نام ژانر
        // ------------------------------------------------

        genre.Name = name;

        await _context.SaveChangesAsync();

        return true;
    }


    // ==================================================
    // فعال / غیرفعال کردن ژانر
    // ==================================================

    public async Task<bool> ChangeStatusAsync(int id)
    {
        var genre = await _context.Genres
            .FirstOrDefaultAsync(x => x.Id == id);

        if (genre == null)
        {
            return false;
        }


        genre.IsActive = !genre.IsActive;

        await _context.SaveChangesAsync();

        return true;
    }
}