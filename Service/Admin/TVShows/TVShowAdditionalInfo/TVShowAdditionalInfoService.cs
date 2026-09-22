using DataBase.Context;
using DataBase.Entity;
using Ganss.Xss;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
namespace Service.Admin.TVShows.TVShowAdditionalInfo;

public interface ITVShowAdditionalInfoService
{
    Task<List<TVShowAdditionalInfoListDto>> GetAllAsync();

    Task<TVShowAdditionalInfoCreateDto> GetCreateDataAsync();

    Task FillCreateFormDataAsync(
        TVShowAdditionalInfoCreateDto dto);

    Task<bool> CreateAsync(
        TVShowAdditionalInfoCreateDto dto);

    Task<TVShowAdditionalInfoEditDto?> GetEditDataAsync(
        int id);

    Task<bool> UpdateAsync(
        TVShowAdditionalInfoEditDto dto);

    Task<TVShowAdditionalInfoDetailsDto?> GetDetailsAsync(
        int id);
}

public class TVShowAdditionalInfoService
    : ITVShowAdditionalInfoService
{
    private readonly MyContext _context;

    public TVShowAdditionalInfoService(MyContext context)
    {
        _context = context;
    }

    public async Task<List<TVShowAdditionalInfoListDto>>
        GetAllAsync()
    {
        return await _context.MovieAdditionalInfos
            .AsNoTracking()
            .Where(x => x.Movie is DataBase.Entity.TVShow)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new TVShowAdditionalInfoListDto
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

    public async Task<TVShowAdditionalInfoCreateDto>
        GetCreateDataAsync()
    {
        var dto = new TVShowAdditionalInfoCreateDto();

        await FillCreateFormDataAsync(dto);

        return dto;
    }

    public async Task FillCreateFormDataAsync(
        TVShowAdditionalInfoCreateDto dto)
    {
        dto.TVShows = await _context.TVShows
            .AsNoTracking()
            .OrderBy(x => x.Title)
            .Select(x => new TVShowAdditionalInfoTVShowItemDto
            {
                Id = x.Id,
                Title = x.Title
            })
            .ToListAsync();
    }

    public async Task<bool> CreateAsync(
        TVShowAdditionalInfoCreateDto dto)
    {
        var tvShowExists = await _context.TVShows
            .AnyAsync(x => x.Id == dto.TVShowId);

        if (!tvShowExists)
            return false;

        var additionalInfoExists =
            await _context.MovieAdditionalInfos
                .AnyAsync(x => x.MovieId == dto.TVShowId);

        if (additionalInfoExists)
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

        var additionalInfo = new MovieAdditionalInfo
        {
            MovieId = dto.TVShowId,
            Content = sanitizedContent
        };

        _context.MovieAdditionalInfos.Add(additionalInfo);

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<TVShowAdditionalInfoEditDto?>
        GetEditDataAsync(int id)
    {
        return await _context.MovieAdditionalInfos
            .AsNoTracking()
            .Where(x =>
                x.Id == id &&
                x.Movie is DataBase.Entity.TVShow)
            .Select(x => new TVShowAdditionalInfoEditDto
            {
                Id = x.Id,
                TVShowId = x.MovieId,
                TVShowTitle = x.Movie.Title,
                Content = x.Content
            })
            .FirstOrDefaultAsync();
    }

    public async Task<bool> UpdateAsync(
        TVShowAdditionalInfoEditDto dto)
    {
        var additionalInfo =
            await _context.MovieAdditionalInfos
                .FirstOrDefaultAsync(x =>
                    x.Id == dto.Id &&
                    x.Movie is DataBase.Entity.TVShow);

        if (additionalInfo == null)
            return false;

        var tvShowExists = await _context.TVShows
            .AnyAsync(x => x.Id == dto.TVShowId);

        if (!tvShowExists)
            return false;

        var duplicateExists =
            await _context.MovieAdditionalInfos
                .AnyAsync(x =>
                    x.MovieId == dto.TVShowId &&
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

        additionalInfo.MovieId = dto.TVShowId;
        additionalInfo.Content = sanitizedContent;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<TVShowAdditionalInfoDetailsDto?>
        GetDetailsAsync(int id)
    {
        return await _context.MovieAdditionalInfos
            .AsNoTracking()
            .Where(x =>
                x.Id == id &&
                x.Movie is DataBase.Entity.TVShow)
            .Select(x => new TVShowAdditionalInfoDetailsDto
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