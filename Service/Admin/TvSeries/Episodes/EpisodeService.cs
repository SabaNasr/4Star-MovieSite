using DataBase.Context;
using DataBase.Entity;
using Microsoft.EntityFrameworkCore;
namespace Service.Admin.TvSeries.Episodes;

// ==================================================
// Interface 
// ==================================================
public interface IEpisodeService
{
    Task<List<EpisodeListDto>> GetAllAsync();

    Task<EpisodeCreateDto> GetCreateDataAsync();

    Task FillCreateFormDataAsync(EpisodeCreateDto dto);

    Task<bool> CreateAsync(EpisodeCreateDto dto);

    Task<EpisodeEditDto?> GetEditDataAsync(int id);

    Task<bool> UpdateAsync(EpisodeEditDto dto);

    Task<EpisodeDetailsDto?> GetDetailsAsync(int id);

    Task<bool> ChangeStatusAsync(int id);
}

public class EpisodeService : IEpisodeService
{
    private readonly MyContext _context;

    public EpisodeService(MyContext context)
    {
        _context = context;
    }

    public async Task<List<EpisodeListDto>> GetAllAsync()
    {
        return await _context.Episodes
            .AsNoTracking()
            .OrderBy(x => x.Season.Series.Title)
            .ThenBy(x => x.Season.Name)
            .ThenBy(x => x.Name)
            .Select(x => new EpisodeListDto
            {
                Id = x.Id,
                Name = x.Name,
                DownloadLink = x.DownloadLink,
                SeasonId = x.SeasonId,
                SeasonName = x.Season.Name,
                SeriesId = x.Season.SeriesId,
                SeriesTitle = x.Season.Series.Title,
                IsActive = x.IsActive,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            })
            .ToListAsync();
    }

    public Task<EpisodeCreateDto> GetCreateDataAsync()
    {
        var dto = new EpisodeCreateDto
        {
            IsActive = true
        };

        return Task.FromResult(dto);
    }

    public async Task FillCreateFormDataAsync(EpisodeCreateDto dto)
    {
        dto.Seasons = await _context.Seasons
            .AsNoTracking()
            .Where(x =>
                x.IsActive &&
                !x.IsArchived &&
                x.Series.IsActive &&
                !x.Series.IsArchived)
            .OrderBy(x => x.Series.Title)
            .ThenBy(x => x.Name)
            .Select(x => new EpisodeSeasonItemDto
            {
                Id = x.Id,
                Name = x.Name,
                SeriesTitle = x.Series.Title
            })
            .ToListAsync();
    }

    public async Task<bool> CreateAsync(EpisodeCreateDto dto)
    {
        var seasonExists = await _context.Seasons
            .AnyAsync(x => x.Id == dto.SeasonId);

        if (!seasonExists)
            return false;

        var name = dto.Name.Trim();

        var nameExists = await _context.Episodes
            .AnyAsync(x =>
                x.SeasonId == dto.SeasonId &&
                x.Name == name);

        if (nameExists)
            return false;

        var episode = new Episode
        {
            SeasonId = dto.SeasonId,
            Name = name,
            DownloadLink = dto.DownloadLink.Trim(),
            IsActive = dto.IsActive
        };

        _context.Episodes.Add(episode);

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<EpisodeEditDto?> GetEditDataAsync(int id)
    {
        return await _context.Episodes
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new EpisodeEditDto
            {
                Id = x.Id,
                SeasonId = x.SeasonId,
                SeasonName = x.Season.Name,
                SeriesTitle = x.Season.Series.Title,
                Name = x.Name,
                DownloadLink = x.DownloadLink,
                IsActive = x.IsActive
            })
            .FirstOrDefaultAsync();
    }

    public async Task<bool> UpdateAsync(EpisodeEditDto dto)
    {
        var episode = await _context.Episodes
            .FirstOrDefaultAsync(x => x.Id == dto.Id);

        if (episode == null)
            return false;

        var seasonExists = await _context.Seasons
            .AnyAsync(x => x.Id == dto.SeasonId);

        if (!seasonExists)
            return false;

        var name = dto.Name.Trim();

        var nameExists = await _context.Episodes
            .AnyAsync(x =>
                x.Id != dto.Id &&
                x.SeasonId == dto.SeasonId &&
                x.Name == name);

        if (nameExists)
            return false;

        episode.SeasonId = dto.SeasonId;
        episode.Name = name;
        episode.DownloadLink = dto.DownloadLink.Trim();
        episode.IsActive = dto.IsActive;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<EpisodeDetailsDto?> GetDetailsAsync(int id)
    {
        return await _context.Episodes
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new EpisodeDetailsDto
            {
                Id = x.Id,
                Name = x.Name,
                DownloadLink = x.DownloadLink,
                SeasonId = x.SeasonId,
                SeasonName = x.Season.Name,
                SeriesId = x.Season.SeriesId,
                SeriesTitle = x.Season.Series.Title,
                IsActive = x.IsActive,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            })
            .FirstOrDefaultAsync();
    }

    public async Task<bool> ChangeStatusAsync(int id)
    {
        var episode = await _context.Episodes
            .FirstOrDefaultAsync(x => x.Id == id);

        if (episode == null)
            return false;

        episode.IsActive = !episode.IsActive;

        await _context.SaveChangesAsync();

        return true;
    }
}