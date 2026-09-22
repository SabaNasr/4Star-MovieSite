using DataBase.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Service.Extention.ImageExtention;
namespace Service.Admin.TVShows.TVShowEpisode;

// ==================================================
// Interface
// ==================================================
public interface ITVShowEpisodeService
{
    Task<List<TVShowEpisodeListDto>> GetAllAsync();

    Task<TVShowEpisodeCreateDto> GetCreateDataAsync();

    Task FillCreateFormDataAsync(
        TVShowEpisodeCreateDto dto);

    Task<bool> CreateAsync(
        TVShowEpisodeCreateDto dto);

    Task<TVShowEpisodeEditDto?> GetEditDataAsync(
        int id);

    Task<bool> UpdateAsync(
        TVShowEpisodeEditDto dto);

    Task<TVShowEpisodeDetailsDto?> GetDetailsAsync(
        int id);

    Task<bool> DeleteAsync(int id);

    Task<bool> ChangeStatusAsync(int id);
}


public class TVShowEpisodeService : ITVShowEpisodeService
{
    private readonly MyContext _context;
    private readonly IWebHostEnvironment _environment;


    public TVShowEpisodeService(
        MyContext context,
        IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }


    // ==================================================
    // Get All
    // ==================================================

    public async Task<List<TVShowEpisodeListDto>> GetAllAsync()
    {
        return await _context.TVShowEpisodes
            .AsNoTracking()
            .OrderBy(x => x.TVShow.Title)
            .ThenBy(x => x.SeasonNumber)
            .ThenBy(x => x.EpisodeNumber)
            .Select(x => new TVShowEpisodeListDto
            {
                Id = x.Id,
                SeasonNumber = x.SeasonNumber,
                EpisodeNumber = x.EpisodeNumber,
                Name = x.Name,
                ImagePath = x.ImagePath,
                DownloadLink = x.DownloadLink,
                TVShowId = x.TVShowId,
                TVShowTitle = x.TVShow.Title,
                IsActive = x.IsActive,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            })
            .ToListAsync();
    }


    // ==================================================
    // Get Create Data
    // ==================================================

    public async Task<TVShowEpisodeCreateDto>
        GetCreateDataAsync()
    {
        var dto = new TVShowEpisodeCreateDto();

        await FillCreateFormDataAsync(dto);

        return dto;
    }


    // ==================================================
    // Fill Create Form
    // ==================================================

    public async Task FillCreateFormDataAsync(
        TVShowEpisodeCreateDto dto)
    {
        dto.TVShows = await _context.TVShows
            .AsNoTracking()
            .OrderBy(x => x.Title)
            .Select(x => new TVShowEpisodeTVShowItemDto
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
        TVShowEpisodeCreateDto dto)
    {
        var tvShowExists = await _context.TVShows
            .AnyAsync(x => x.Id == dto.TVShowId);

        if (!tvShowExists)
            return false;


        var duplicateExists =
            await _context.TVShowEpisodes
                .AnyAsync(x =>
                    x.TVShowId == dto.TVShowId &&
                    x.SeasonNumber == dto.SeasonNumber &&
                    x.EpisodeNumber == dto.EpisodeNumber);

        if (duplicateExists)
            return false;


        if (dto.Image == null ||
            dto.Image.Length == 0)
        {
            return false;
        }


        var imagePath = await dto.Image.SaveImageAsync(
            _environment.WebRootPath,
            "uploads",
            "tvshows/episodes");


        if (string.IsNullOrWhiteSpace(imagePath))
            return false;


        var episode = new DataBase.Entity.TVShowEpisode
        {
            TVShowId = dto.TVShowId,
            SeasonNumber = dto.SeasonNumber,
            EpisodeNumber = dto.EpisodeNumber,
            Name = dto.Name.Trim(),
            ImagePath = imagePath,
            DownloadLink = dto.DownloadLink.Trim(),
            IsActive = dto.IsActive
        };


        _context.TVShowEpisodes.Add(episode);

        await _context.SaveChangesAsync();

        return true;
    }


    // ==================================================
    // Get Edit Data
    // ==================================================

    public async Task<TVShowEpisodeEditDto?>
        GetEditDataAsync(int id)
    {
        return await _context.TVShowEpisodes
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new TVShowEpisodeEditDto
            {
                Id = x.Id,
                TVShowId = x.TVShowId,
                TVShowTitle = x.TVShow.Title,
                SeasonNumber = x.SeasonNumber,
                EpisodeNumber = x.EpisodeNumber,
                Name = x.Name,
                ExistingImagePath = x.ImagePath,
                DownloadLink = x.DownloadLink,
                IsActive = x.IsActive
            })
            .FirstOrDefaultAsync();
    }


    // ==================================================
    // Update
    // ==================================================

    public async Task<bool> UpdateAsync(
        TVShowEpisodeEditDto dto)
    {
        var episode = await _context.TVShowEpisodes
            .FirstOrDefaultAsync(x => x.Id == dto.Id);

        if (episode == null)
            return false;


        var tvShowExists = await _context.TVShows
            .AnyAsync(x => x.Id == dto.TVShowId);

        if (!tvShowExists)
            return false;


        var duplicateExists =
            await _context.TVShowEpisodes
                .AnyAsync(x =>
                    x.Id != dto.Id &&
                    x.TVShowId == dto.TVShowId &&
                    x.SeasonNumber == dto.SeasonNumber &&
                    x.EpisodeNumber == dto.EpisodeNumber);

        if (duplicateExists)
            return false;


        episode.TVShowId = dto.TVShowId;
        episode.SeasonNumber = dto.SeasonNumber;
        episode.EpisodeNumber = dto.EpisodeNumber;
        episode.Name = dto.Name.Trim();
        episode.DownloadLink = dto.DownloadLink.Trim();
        episode.IsActive = dto.IsActive;


        if (dto.Image != null &&
            dto.Image.Length > 0)
        {
            var imagePath = await dto.Image.SaveImageAsync(
                _environment.WebRootPath,
                "uploads",
                "tvshows/episodes");

            if (!string.IsNullOrWhiteSpace(imagePath))
            {
                episode.ImagePath = imagePath;
            }
        }


        await _context.SaveChangesAsync();

        return true;
    }


    // ==================================================
    // Details
    // ==================================================

    public async Task<TVShowEpisodeDetailsDto?>
        GetDetailsAsync(int id)
    {
        return await _context.TVShowEpisodes
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new TVShowEpisodeDetailsDto
            {
                Id = x.Id,
                TVShowId = x.TVShowId,
                TVShowTitle = x.TVShow.Title,
                SeasonNumber = x.SeasonNumber,
                EpisodeNumber = x.EpisodeNumber,
                Name = x.Name,
                ImagePath = x.ImagePath,
                DownloadLink = x.DownloadLink,
                IsActive = x.IsActive,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            })
            .FirstOrDefaultAsync();
    }


    // ==================================================
    // Delete
    // ==================================================

    public async Task<bool> DeleteAsync(int id)
    {
        var episode = await _context.TVShowEpisodes
            .FirstOrDefaultAsync(x => x.Id == id);

        if (episode == null)
            return false;


        _context.TVShowEpisodes.Remove(episode);

        await _context.SaveChangesAsync();

        return true;
    }


    // ==================================================
    // Change Status
    // ==================================================

    public async Task<bool> ChangeStatusAsync(int id)
    {
        var episode = await _context.TVShowEpisodes
            .FirstOrDefaultAsync(x => x.Id == id);

        if (episode == null)
            return false;


        episode.IsActive = !episode.IsActive;

        await _context.SaveChangesAsync();

        return true;
    }
}