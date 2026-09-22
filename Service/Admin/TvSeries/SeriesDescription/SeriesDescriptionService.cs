using DataBase.Context;
using DataBase.Entity;
using Ganss.Xss;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
namespace Service.Admin.TvSeries.SeriesDescription;

// ==================================================
// Interface 
// ==================================================
public interface ISeriesDescriptionService
{
    Task<List<SeriesDescriptionListDto>> GetAllAsync();

    Task<SeriesDescriptionCreateDto>
        GetCreateDataAsync();

    Task FillCreateFormDataAsync(
        SeriesDescriptionCreateDto dto);

    Task<bool> CreateAsync(
        SeriesDescriptionCreateDto dto);

    Task<SeriesDescriptionEditDto?>
        GetEditDataAsync(int id);

    Task<bool> UpdateAsync(
        SeriesDescriptionEditDto dto);

    Task<SeriesDescriptionDetailsDto?>
        GetDetailsAsync(int id);
}

public class SeriesDescriptionService
    : ISeriesDescriptionService
{
    private readonly MyContext _context;

    public SeriesDescriptionService(MyContext context)
    {
        _context = context;
    }

    // ==================================================
    // Get All
    // ==================================================

    public async Task<List<SeriesDescriptionListDto>>
        GetAllAsync()
    {
        return await _context.MovieDescriptions
            .AsNoTracking()
            .Where(x => x.Movie is DataBase.Entity.Series)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new SeriesDescriptionListDto
            {
                Id = x.Id,
                SeriesId = x.MovieId,
                SeriesTitle = x.Movie.Title,
                IsActive = x.IsActive,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            })
            .ToListAsync();
    }

    // ==================================================
    // Get Create Data
    // ==================================================

    public async Task<SeriesDescriptionCreateDto>
        GetCreateDataAsync()
    {
        var dto = new SeriesDescriptionCreateDto();

        await FillCreateFormDataAsync(dto);

        return dto;
    }

    // ==================================================
    // Fill Create Form Data
    // ==================================================

    public async Task FillCreateFormDataAsync(
        SeriesDescriptionCreateDto dto)
    {
        dto.Series = await _context.Series
            .AsNoTracking()
            .OrderBy(x => x.Title)
            .Select(x => new SeriesDescriptionSeriesItemDto
            {
                Id = x.Id,
                Title = x.Title
            })
            .ToListAsync();
    }

    // ==================================================
    // Create
    // ==================================================

    public async Task<bool> CreateAsync(
        SeriesDescriptionCreateDto dto)
    {
        var seriesExists = await _context.Series
            .AnyAsync(x => x.Id == dto.SeriesId);

        if (!seriesExists)
            return false;

        var descriptionExists =
            await _context.MovieDescriptions
                .AnyAsync(x =>
                    x.MovieId == dto.SeriesId);

        if (descriptionExists)
            return false;

        var sanitizer = new HtmlSanitizer();

        var sanitizedContent =
            sanitizer.Sanitize(dto.Content);

        var plainText = Regex
            .Replace(
                sanitizedContent,
                "<.*?>",
                string.Empty)
            .Trim();

        if (string.IsNullOrWhiteSpace(plainText))
            return false;

        var description = new MovieDescription
        {
            MovieId = dto.SeriesId,
            Content = sanitizedContent
        };

        _context.MovieDescriptions.Add(description);

        await _context.SaveChangesAsync();

        return true;
    }

    // ==================================================
    // Get Edit Data
    // ==================================================

    public async Task<SeriesDescriptionEditDto?>
        GetEditDataAsync(int id)
    {
        return await _context.MovieDescriptions
            .AsNoTracking()
            .Where(x =>
              x.Id == id &&
              x.Movie is DataBase.Entity.Series)
            .Select(x => new SeriesDescriptionEditDto
            {
                Id = x.Id,
                SeriesId = x.MovieId,
                SeriesTitle = x.Movie.Title,
                Content = x.Content
            })
            .FirstOrDefaultAsync();
    }

    // ==================================================
    // Update
    // ==================================================

    public async Task<bool> UpdateAsync(
        SeriesDescriptionEditDto dto)
    {
        var description =
            await _context.MovieDescriptions
                .FirstOrDefaultAsync(x =>
                    x.Id == dto.Id &&
                    x.Movie is DataBase.Entity.Series);

        if (description == null)
            return false;

        var seriesExists = await _context.Series
            .AnyAsync(x => x.Id == dto.SeriesId);

        if (!seriesExists)
            return false;

        var duplicateExists =
            await _context.MovieDescriptions
                .AnyAsync(x =>
                    x.MovieId == dto.SeriesId &&
                    x.Id != dto.Id);

        if (duplicateExists)
            return false;

        var sanitizer = new HtmlSanitizer();

        var sanitizedContent =
            sanitizer.Sanitize(dto.Content);

        var plainText = Regex
            .Replace(
                sanitizedContent,
                "<.*?>",
                string.Empty)
            .Trim();

        if (string.IsNullOrWhiteSpace(plainText))
            return false;

        description.MovieId = dto.SeriesId;
        description.Content = sanitizedContent;

        await _context.SaveChangesAsync();

        return true;
    }

    // ==================================================
    // Get Details
    // ==================================================

    public async Task<SeriesDescriptionDetailsDto?>
        GetDetailsAsync(int id)
    {
        return await _context.MovieDescriptions
            .AsNoTracking()
            .Where(x =>
                x.Id == id &&
                x.Movie is DataBase.Entity.Series)
            .Select(x => new SeriesDescriptionDetailsDto
            {
                Id = x.Id,
                SeriesId = x.MovieId,
                SeriesTitle = x.Movie.Title,
                Content = x.Content,
                IsActive = x.IsActive,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            })
            .FirstOrDefaultAsync();
    }
}