using DataBase.Context;
using DataBase.Entity;
using Ganss.Xss;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
namespace Service.Admin.TVShows.TVShowDescription;

// ==================================================
// Interface
// ==================================================
public interface ITVShowDescriptionService
{
    Task<List<TVShowDescriptionListDto>> GetAllAsync();

    Task<TVShowDescriptionCreateDto> GetCreateDataAsync();

    Task FillCreateFormDataAsync(
        TVShowDescriptionCreateDto dto);

    Task<bool> CreateAsync(
        TVShowDescriptionCreateDto dto);

    Task<TVShowDescriptionEditDto?> GetEditDataAsync(
        int id);

    Task<bool> UpdateAsync(
        TVShowDescriptionEditDto dto);

    Task<TVShowDescriptionDetailsDto?> GetDetailsAsync(
        int id);
}


public class TVShowDescriptionService
    : ITVShowDescriptionService
{
    private readonly MyContext _context;


    public TVShowDescriptionService(
        MyContext context)
    {
        _context = context;
    }


    // ==================================================
    // Get All
    // ==================================================

    public async Task<List<TVShowDescriptionListDto>>
        GetAllAsync()
    {
        return await _context.MovieDescriptions
            .AsNoTracking()
            .Where(x =>
                x.Movie is DataBase.Entity.TVShow)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new TVShowDescriptionListDto
            {
                Id = x.Id,
                TVShowId = x.MovieId,
                TVShowTitle = x.Movie.Title,
                IsActive = x.IsActive,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            })
            .ToListAsync();
    }


    // ==================================================
    // Get Create Data
    // ==================================================

    public async Task<TVShowDescriptionCreateDto>
        GetCreateDataAsync()
    {
        var dto =
            new TVShowDescriptionCreateDto();

        await FillCreateFormDataAsync(dto);

        return dto;
    }


    // ==================================================
    // Fill Create Form
    // ==================================================

    public async Task FillCreateFormDataAsync(
        TVShowDescriptionCreateDto dto)
    {
        dto.TVShows = await _context.TVShows
            .AsNoTracking()
            .OrderBy(x => x.Title)
            .Select(x => new TVShowDescriptionTVShowItemDto
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
        TVShowDescriptionCreateDto dto)
    {
        var tvShowExists =
            await _context.TVShows
                .AnyAsync(x => x.Id == dto.TVShowId);

        if (!tvShowExists)
            return false;


        var descriptionExists =
            await _context.MovieDescriptions
                .AnyAsync(x =>
                    x.MovieId == dto.TVShowId);

        if (descriptionExists)
            return false;


        var sanitizer =
            new HtmlSanitizer();


        var sanitizedContent =
            sanitizer.Sanitize(dto.Content);


        var plainText =
            Regex.Replace(
                sanitizedContent,
                "<.*?>",
                string.Empty)
            .Trim();


        if (string.IsNullOrWhiteSpace(plainText))
            return false;


        var description =
            new MovieDescription
            {
                MovieId = dto.TVShowId,
                Content = sanitizedContent
            };


        _context.MovieDescriptions.Add(description);

        await _context.SaveChangesAsync();

        return true;
    }


    // ==================================================
    // Get Edit Data
    // ==================================================

    public async Task<TVShowDescriptionEditDto?>
        GetEditDataAsync(int id)
    {
        return await _context.MovieDescriptions
            .AsNoTracking()
            .Where(x =>
                x.Id == id &&
                x.Movie is DataBase.Entity.TVShow)
            .Select(x => new TVShowDescriptionEditDto
            {
                Id = x.Id,
                TVShowId = x.MovieId,
                TVShowTitle = x.Movie.Title,
                Content = x.Content
            })
            .FirstOrDefaultAsync();
    }


    // ==================================================
    // Update
    // ==================================================

    public async Task<bool> UpdateAsync(
        TVShowDescriptionEditDto dto)
    {
        var description =
            await _context.MovieDescriptions
                .FirstOrDefaultAsync(x =>
                    x.Id == dto.Id &&
                    x.Movie is DataBase.Entity.TVShow);


        if (description == null)
            return false;


        var tvShowExists =
            await _context.TVShows
                .AnyAsync(x => x.Id == dto.TVShowId);

        if (!tvShowExists)
            return false;


        var duplicateExists =
            await _context.MovieDescriptions
                .AnyAsync(x =>
                    x.MovieId == dto.TVShowId &&
                    x.Id != dto.Id);


        if (duplicateExists)
            return false;


        var sanitizer =
            new HtmlSanitizer();


        var sanitizedContent =
            sanitizer.Sanitize(dto.Content);


        var plainText =
            Regex.Replace(
                sanitizedContent,
                "<.*?>",
                string.Empty)
            .Trim();


        if (string.IsNullOrWhiteSpace(plainText))
            return false;


        description.MovieId =
            dto.TVShowId;

        description.Content =
            sanitizedContent;


        await _context.SaveChangesAsync();

        return true;
    }


    // ==================================================
    // Details
    // ==================================================

    public async Task<TVShowDescriptionDetailsDto?>
        GetDetailsAsync(int id)
    {
        return await _context.MovieDescriptions
            .AsNoTracking()
            .Where(x =>
                x.Id == id &&
                x.Movie is DataBase.Entity.TVShow)
            .Select(x => new TVShowDescriptionDetailsDto
            {
                Id = x.Id,
                TVShowId = x.MovieId,
                TVShowTitle = x.Movie.Title,
                Content = x.Content,
                IsActive = x.IsActive,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            })
            .FirstOrDefaultAsync();
    }
}
