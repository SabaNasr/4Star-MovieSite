using DataBase.Context;
using DataBase.Entity;
using Microsoft.EntityFrameworkCore;
namespace Service.Admin.TvSeries.Seasons;

// ==================================================
// Interface
// ==================================================
public interface ISeasonService
{
    Task<List<SeasonListDto>> GetAllAsync();

    Task<SeasonCreateDto> GetCreateDataAsync();

    Task FillCreateFormDataAsync(SeasonCreateDto dto);

    Task<bool> CreateAsync(SeasonCreateDto dto);

    Task<SeasonEditDto?> GetEditDataAsync(int id);

    Task<bool> UpdateAsync(SeasonEditDto dto);

    Task<SeasonDetailsDto?> GetDetailsAsync(int id);

    Task<bool> ChangeStatusAsync(int id);
}

public class SeasonService : ISeasonService
{
    private readonly MyContext _context;

    public SeasonService(MyContext context)
    {
        _context = context;
    }

    public async Task<List<SeasonListDto>> GetAllAsync()
    {
        return await _context.Seasons
            .AsNoTracking()
            .OrderBy(x => x.Series.Title)
            .ThenBy(x => x.Name)
            .Select(x => new SeasonListDto
            {
                Id = x.Id,
                Name = x.Name,
                DownloadLink = x.DownloadLink,
                SeriesId = x.SeriesId,
                SeriesTitle = x.Series.Title,
                EpisodeCount = x.Episodes.Count,
                IsActive = x.IsActive,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            })
            .ToListAsync();
    }

    public Task<SeasonCreateDto> GetCreateDataAsync()
    {
        var dto = new SeasonCreateDto
        {
            IsActive = true
        };

        return Task.FromResult(dto);
    }

    public async Task FillCreateFormDataAsync(SeasonCreateDto dto)
    {
        dto.Series = await _context.Series
            .AsNoTracking()
            .Where(x => x.IsActive && !x.IsArchived)
            .OrderBy(x => x.Title)
            .Select(x => new SeasonSeriesItemDto
            {
                Id = x.Id,
                Title = x.Title
            })
            .ToListAsync();
    }

    public async Task<bool> CreateAsync(SeasonCreateDto dto)
    {
        var seriesExists = await _context.Series
            .AnyAsync(x => x.Id == dto.SeriesId);

        if (!seriesExists)
            return false;

        var name = dto.Name.Trim();

        var nameExists = await _context.Seasons
            .AnyAsync(x =>
                x.SeriesId == dto.SeriesId &&
                x.Name == name);

        if (nameExists)
            return false;

        var season = new Season
        {
            SeriesId = dto.SeriesId,
            Name = name,
            DownloadLink = string.IsNullOrWhiteSpace(dto.DownloadLink)
                ? null
                : dto.DownloadLink.Trim(),
            IsActive = dto.IsActive
        };

        _context.Seasons.Add(season);

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<SeasonEditDto?> GetEditDataAsync(int id)
    {
        return await _context.Seasons
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new SeasonEditDto
            {
                Id = x.Id,
                SeriesId = x.SeriesId,
                SeriesTitle = x.Series.Title,
                Name = x.Name,
                DownloadLink = x.DownloadLink,
                IsActive = x.IsActive
            })
            .FirstOrDefaultAsync();
    }

    public async Task<bool> UpdateAsync(SeasonEditDto dto)
    {
        var season = await _context.Seasons
            .FirstOrDefaultAsync(x => x.Id == dto.Id);

        if (season == null)
            return false;

        var seriesExists = await _context.Series
            .AnyAsync(x => x.Id == dto.SeriesId);

        if (!seriesExists)
            return false;

        var name = dto.Name.Trim();

        var nameExists = await _context.Seasons
            .AnyAsync(x =>
                x.Id != dto.Id &&
                x.SeriesId == dto.SeriesId &&
                x.Name == name);

        if (nameExists)
            return false;

        season.SeriesId = dto.SeriesId;
        season.Name = name;
        season.DownloadLink = string.IsNullOrWhiteSpace(dto.DownloadLink)
            ? null
            : dto.DownloadLink.Trim();
        season.IsActive = dto.IsActive;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<SeasonDetailsDto?> GetDetailsAsync(int id)
    {
        return await _context.Seasons
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new SeasonDetailsDto
            {
                Id = x.Id,
                Name = x.Name,
                DownloadLink = x.DownloadLink,
                SeriesId = x.SeriesId,
                SeriesTitle = x.Series.Title,
                EpisodeCount = x.Episodes.Count,
                IsActive = x.IsActive,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            })
            .FirstOrDefaultAsync();
    }

    public async Task<bool> ChangeStatusAsync(int id)
    {
        var season = await _context.Seasons
            .FirstOrDefaultAsync(x => x.Id == id);

        if (season == null)
            return false;

        season.IsActive = !season.IsActive;

        await _context.SaveChangesAsync();

        return true;
    }
}