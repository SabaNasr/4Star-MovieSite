using DataBase.Context;
using DataBase.Entity;
using Ganss.Xss;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
namespace Service.Admin.TvSeries.SeriesAdditionalInfo;

public interface ISeriesAdditionalInfoService
{
    Task<List<SeriesAdditionalInfoListDto>> GetAllAsync();

    Task<SeriesAdditionalInfoCreateDto>
        GetCreateDataAsync();

    Task FillCreateFormDataAsync(
        SeriesAdditionalInfoCreateDto dto);

    Task<bool> CreateAsync(
        SeriesAdditionalInfoCreateDto dto);

    Task<SeriesAdditionalInfoEditDto?>
        GetEditDataAsync(int id);

    Task<bool> UpdateAsync(
        SeriesAdditionalInfoEditDto dto);

    Task<SeriesAdditionalInfoDetailsDto?>
        GetDetailsAsync(int id);
}

public class SeriesAdditionalInfoService
    : ISeriesAdditionalInfoService
{
    private readonly MyContext _context;

    public SeriesAdditionalInfoService(MyContext context)
    {
        _context = context;
    }

    // ==================================================
    // Get All
    // ==================================================

    public async Task<List<SeriesAdditionalInfoListDto>>
        GetAllAsync()
    {
        return await _context.MovieAdditionalInfos
            .AsNoTracking()
            .Where(x =>
                x.Movie is DataBase.Entity.Series)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new SeriesAdditionalInfoListDto
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

    public async Task<SeriesAdditionalInfoCreateDto>
        GetCreateDataAsync()
    {
        var dto =
            new SeriesAdditionalInfoCreateDto();

        await FillCreateFormDataAsync(dto);

        return dto;
    }

    // ==================================================
    // Fill Create Form Data
    // ==================================================

    public async Task FillCreateFormDataAsync(
        SeriesAdditionalInfoCreateDto dto)
    {
        dto.Series = await _context.Series
            .AsNoTracking()
            .OrderBy(x => x.Title)
            .Select(x => new SeriesAdditionalInfoSeriesItemDto
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
        SeriesAdditionalInfoCreateDto dto)
    {
        var seriesExists = await _context.Series
            .AnyAsync(x => x.Id == dto.SeriesId);

        if (!seriesExists)
            return false;

        var additionalInfoExists =
            await _context.MovieAdditionalInfos
                .AnyAsync(x =>
                    x.MovieId == dto.SeriesId);

        if (additionalInfoExists)
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

        var additionalInfo =
            new MovieAdditionalInfo
            {
                MovieId = dto.SeriesId,
                Content = sanitizedContent
            };

        _context.MovieAdditionalInfos
            .Add(additionalInfo);

        await _context.SaveChangesAsync();

        return true;
    }

    // ==================================================
    // Get Edit Data
    // ==================================================

    public async Task<SeriesAdditionalInfoEditDto?>
        GetEditDataAsync(int id)
    {
        return await _context.MovieAdditionalInfos
            .AsNoTracking()
            .Where(x =>
                x.Id == id &&
                x.Movie is DataBase.Entity.Series)
            .Select(x => new SeriesAdditionalInfoEditDto
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
        SeriesAdditionalInfoEditDto dto)
    {
        var additionalInfo =
            await _context.MovieAdditionalInfos
                .FirstOrDefaultAsync(x =>
                    x.Id == dto.Id &&
                    x.Movie is DataBase.Entity.Series);

        if (additionalInfo == null)
            return false;

        var seriesExists = await _context.Series
            .AnyAsync(x =>
                x.Id == dto.SeriesId);

        if (!seriesExists)
            return false;

        var duplicateExists =
            await _context.MovieAdditionalInfos
                .AnyAsync(x =>
                    x.MovieId == dto.SeriesId &&
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

        additionalInfo.MovieId =
            dto.SeriesId;

        additionalInfo.Content =
            sanitizedContent;

        await _context.SaveChangesAsync();

        return true;
    }

    // ==================================================
    // Get Details
    // ==================================================

    public async Task<SeriesAdditionalInfoDetailsDto?>
        GetDetailsAsync(int id)
    {
        return await _context.MovieAdditionalInfos
            .AsNoTracking()
            .Where(x =>
                x.Id == id &&
                x.Movie is DataBase.Entity.Series)
            .Select(x => new SeriesAdditionalInfoDetailsDto
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