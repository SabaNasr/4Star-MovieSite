using DataBase.Context;
using DataBase.Entity;
using DataBase.Enum;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Service.Extention.ImageExtention;
namespace Service.Admin.TVShows.TvShow;

// ==================================================
// Interface
// ==================================================
public interface ITVShowService
{
    Task<List<TVShowListDto>> GetAllAsync();

    Task<TVShowCreateDto> GetCreateDataAsync();

    Task FillCreateFormDataAsync(TVShowCreateDto dto);

    Task<int> CreateAsync(TVShowCreateDto dto);

    Task<TVShowEditDto?> GetEditDataAsync(int id);

    Task FillEditFormDataAsync(TVShowEditDto dto);

    Task<bool> UpdateAsync(TVShowEditDto dto);

    Task<TVShowDetailsDto?> GetDetailsAsync(int id);

    Task<List<TVShowListDto>> GetArchivedAsync();

    Task<List<TVShowListDto>> GetDeletedAsync();

    Task<bool> DeleteAsync(int id);

    Task<bool> RestoreAsync(int id);

    Task<bool> ChangeStatusAsync(int id);

    Task<bool> ArchiveAsync(int id);

    Task<bool> UnarchiveAsync(int id);
}
// ==================================================
// Service
// ==================================================

public class TVShowService : ITVShowService
{
    private readonly MyContext _context;

    private readonly IWebHostEnvironment
        _webHostEnvironment;

    public TVShowService(
        MyContext context,
        IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }


    // ==================================================
    // دریافت تمام TVShow ها
    // ==================================================

    public async Task<List<TVShowListDto>> GetAllAsync()
    {
        return await _context.TVShows
            .AsNoTracking()
            .Where(x => !x.IsArchived)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new TVShowListDto
            {
                Id = x.Id,
                Title = x.Title,
                PosterPath = x.PosterPath,
                Year = x.Year,
                DurationMinutes = x.DurationMinutes,
                ImdbRating = x.ImdbRating,
                ViewCount = x.ViewCount,
                EpisodeCount = x.Episodes.Count,
                IsActive = x.IsActive,
                IsArchived = x.IsArchived,
                CreatedAt = x.CreatedAt,
                ArchivedAt = x.ArchivedAt
            })
            .ToListAsync();
    }


    // ==================================================
    // دریافت اطلاعات اولیه Create
    // ==================================================

    public async Task<TVShowCreateDto> GetCreateDataAsync()
    {
        var dto = new TVShowCreateDto();

        await FillCreateFormDataAsync(dto);

        return dto;
    }


    // ==================================================
    // پر کردن اطلاعات فرم Create
    // ==================================================

    public async Task FillCreateFormDataAsync(
        TVShowCreateDto dto)
    {
        var genres = await _context.Genres
            .AsNoTracking()
            .Where(x => x.IsActive)
            .OrderBy(x => x.Name)
            .ToListAsync();

        var languages = await _context.Languages
            .AsNoTracking()
            .Where(x => x.IsActive)
            .OrderBy(x => x.Name)
            .ToListAsync();

        var castMembers = await _context.CastMembers
            .AsNoTracking()
            .Where(x => x.IsActive)
            .OrderBy(x => x.FullName)
            .ToListAsync();


        dto.Genres = genres
            .Select(x => new MovieSelectionItemDto
            {
                Id = x.Id,
                Name = x.Name,
                IsSelected =
                    dto.SelectedGenreIds.Contains(x.Id)
            })
            .ToList();


        dto.Languages = languages
            .Select(x => new MovieSelectionItemDto
            {
                Id = x.Id,
                Name = x.Name,
                IsSelected =
                    dto.SelectedLanguageIds.Contains(x.Id)
            })
            .ToList();


        dto.CastMembers = castMembers
            .Select(x =>
            {
                var selected =
                    dto.SelectedCastMembers
                        .FirstOrDefault(
                            c => c.CastMemberId == x.Id);

                return new MovieCastMemberSelectionDto
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    PhotoPath = x.PhotoPath,
                    IsSelected = selected != null,
                    CastType =
                        selected?.CastType
                        ?? CastType.Actor
                };
            })
            .ToList();
    }


    // ==================================================
    // ایجاد TVShow
    // ==================================================

    public async Task<int> CreateAsync(
        TVShowCreateDto dto)
    {
        string posterPath = string.Empty;


        // ------------------------------------------------
        // ذخیره پوستر
        // ------------------------------------------------

        if (dto.Poster != null)
        {
            posterPath =
                await dto.Poster.SaveImageAsync(
                    _webHostEnvironment.WebRootPath,
                    "movies",
                    "posters");
        }


        // ------------------------------------------------
        // ایجاد TVShow
        // ------------------------------------------------

        var tvShow = new TVShow
        {
            Title = dto.Title.Trim(),
            PosterPath = posterPath,
            ShortDescription =
                dto.ShortDescription.Trim(),

            DownloadLink =
                string.IsNullOrWhiteSpace(
                    dto.DownloadLink)
                    ? null
                    : dto.DownloadLink.Trim(),

            DurationMinutes =
                dto.DurationMinutes,

            Year = dto.Year,

            ImdbRating =
                dto.ImdbRating,

            ViewCount =
                dto.ViewCount,

            AvailableQualities =
                dto.AvailableQualities
                    .Distinct()
                    .ToList()
        };


        // ------------------------------------------------
        // دریافت ژانرهای انتخاب شده
        // ------------------------------------------------

        if (dto.SelectedGenreIds.Count > 0)
        {
            tvShow.Genres =
                await _context.Genres
                    .Where(x =>
                        dto.SelectedGenreIds
                            .Contains(x.Id))
                    .ToListAsync();
        }


        // ------------------------------------------------
        // دریافت زبان‌های انتخاب شده
        // ------------------------------------------------

        if (dto.SelectedLanguageIds.Count > 0)
        {
            tvShow.Languages =
                await _context.Languages
                    .Where(x =>
                        dto.SelectedLanguageIds
                            .Contains(x.Id))
                    .ToListAsync();
        }


        // ------------------------------------------------
        // دریافت عوامل انتخاب شده
        // ------------------------------------------------

        var selectedCastMemberIds =
            dto.SelectedCastMembers
                .Select(x => x.CastMemberId)
                .Distinct()
                .ToList();

        if (selectedCastMemberIds.Count > 0)
        {
            var selectedCastMembers =
                await _context.CastMembers
                    .Where(x =>
                        selectedCastMemberIds
                            .Contains(x.Id))
                    .ToListAsync();

            foreach (var castMember
                     in selectedCastMembers)
            {
                tvShow.CastMembers.Add(
                    castMember);
            }
        }


        // ------------------------------------------------
        // ثبت TVShow
        // ------------------------------------------------

        _context.TVShows.Add(tvShow);

        await _context.SaveChangesAsync();


        // ------------------------------------------------
        // ثبت نقش عوامل
        // ------------------------------------------------

        await SaveCastMemberRolesAsync(
            tvShow.Id,
            dto.SelectedCastMembers);


        return tvShow.Id;
    }


    // ==================================================
    // دریافت اطلاعات TVShow برای Edit
    // ==================================================

    public async Task<TVShowEditDto?> GetEditDataAsync(
        int id)
    {
        var tvShow = await _context.TVShows
            .Include(x => x.Genres)
            .Include(x => x.Languages)
            .Include(x => x.CastMembers)
            .FirstOrDefaultAsync(x => x.Id == id);


        if (tvShow == null)
            return null;


        var selectedCastMembers =
            await GetTVShowCastMembersAsync(
                tvShow.Id);


        var dto = new TVShowEditDto
        {
            Id = tvShow.Id,

            Title = tvShow.Title,

            ExistingPosterPath =
                tvShow.PosterPath,

            ShortDescription =
                tvShow.ShortDescription,

            DownloadLink =
                tvShow.DownloadLink,

            DurationMinutes =
                tvShow.DurationMinutes,

            Year =
                tvShow.Year,

            ImdbRating =
                tvShow.ImdbRating,

            ViewCount =
                tvShow.ViewCount,

            SelectedGenreIds =
                tvShow.Genres
                    .Select(x => x.Id)
                    .ToList(),

            SelectedLanguageIds =
                tvShow.Languages
                    .Select(x => x.Id)
                    .ToList(),

            SelectedCastMembers =
                selectedCastMembers,

            AvailableQualities =
                tvShow.AvailableQualities
                    .ToList()
        };


        await FillEditFormDataAsync(dto);

        return dto;
    }


    // ==================================================
    // پر کردن اطلاعات فرم Edit
    // ==================================================

    public async Task FillEditFormDataAsync(
        TVShowEditDto dto)
    {
        var genres = await _context.Genres
            .AsNoTracking()
            .Where(x => x.IsActive)
            .OrderBy(x => x.Name)
            .ToListAsync();

        var languages = await _context.Languages
            .AsNoTracking()
            .Where(x => x.IsActive)
            .OrderBy(x => x.Name)
            .ToListAsync();

        var castMembers = await _context.CastMembers
            .AsNoTracking()
            .Where(x => x.IsActive)
            .OrderBy(x => x.FullName)
            .ToListAsync();


        dto.Genres = genres
            .Select(x => new MovieSelectionItemDto
            {
                Id = x.Id,
                Name = x.Name,
                IsSelected =
                    dto.SelectedGenreIds.Contains(x.Id)
            })
            .ToList();


        dto.Languages = languages
            .Select(x => new MovieSelectionItemDto
            {
                Id = x.Id,
                Name = x.Name,
                IsSelected =
                    dto.SelectedLanguageIds.Contains(x.Id)
            })
            .ToList();


        dto.CastMembers = castMembers
            .Select(x =>
            {
                var selected =
                    dto.SelectedCastMembers
                        .FirstOrDefault(
                            c => c.CastMemberId == x.Id);

                return new MovieCastMemberSelectionDto
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    PhotoPath = x.PhotoPath,
                    IsSelected = selected != null,
                    CastType =
                        selected?.CastType
                        ?? CastType.Actor
                };
            })
            .ToList();
    }


    // ==================================================
    // ویرایش TVShow
    // ==================================================

    public async Task<bool> UpdateAsync(
        TVShowEditDto dto)
    {
        var tvShow = await _context.TVShows
            .Include(x => x.Genres)
            .Include(x => x.Languages)
            .Include(x => x.CastMembers)
            .FirstOrDefaultAsync(x => x.Id == dto.Id);


        if (tvShow == null)
            return false;


        // ------------------------------------------------
        // بروزرسانی اطلاعات اصلی
        // ------------------------------------------------

        tvShow.Title =
            dto.Title.Trim();

        tvShow.ShortDescription =
            dto.ShortDescription.Trim();

        tvShow.DownloadLink =
            string.IsNullOrWhiteSpace(
                dto.DownloadLink)
                ? null
                : dto.DownloadLink.Trim();

        tvShow.DurationMinutes =
            dto.DurationMinutes;

        tvShow.Year =
            dto.Year;

        tvShow.ImdbRating =
            dto.ImdbRating;

        tvShow.ViewCount =
            dto.ViewCount;

        tvShow.AvailableQualities =
            dto.AvailableQualities
                .Distinct()
                .ToList();


        // ------------------------------------------------
        // اگر پوستر جدید ارسال شده باشد
        // ------------------------------------------------

        if (dto.Poster != null)
        {
            var newPosterPath =
                await dto.Poster.SaveImageAsync(
                    _webHostEnvironment.WebRootPath,
                    "movies",
                    "posters");


            if (!string.IsNullOrWhiteSpace(
                tvShow.PosterPath))
            {
                var oldFilePath =
                    Path.Combine(
                        _webHostEnvironment.WebRootPath,
                        tvShow.PosterPath
                            .TrimStart('/', '\\'));


                if (File.Exists(oldFilePath))
                {
                    File.Delete(oldFilePath);
                }
            }


            tvShow.PosterPath =
                newPosterPath;
        }


        // ------------------------------------------------
        // بروزرسانی ژانرها
        // ------------------------------------------------

        tvShow.Genres.Clear();

        if (dto.SelectedGenreIds.Count > 0)
        {
            var selectedGenres =
                await _context.Genres
                    .Where(x =>
                        dto.SelectedGenreIds
                            .Contains(x.Id))
                    .ToListAsync();

            foreach (var genre
                     in selectedGenres)
            {
                tvShow.Genres.Add(genre);
            }
        }


        // ------------------------------------------------
        // بروزرسانی زبان‌ها
        // ------------------------------------------------

        tvShow.Languages.Clear();

        if (dto.SelectedLanguageIds.Count > 0)
        {
            var selectedLanguages =
                await _context.Languages
                    .Where(x =>
                        dto.SelectedLanguageIds
                            .Contains(x.Id))
                    .ToListAsync();

            foreach (var language
                     in selectedLanguages)
            {
                tvShow.Languages.Add(language);
            }
        }


        // ------------------------------------------------
        // بروزرسانی عوامل
        // ------------------------------------------------

        tvShow.CastMembers.Clear();

        var selectedCastMemberIds =
            dto.SelectedCastMembers
                .Select(x => x.CastMemberId)
                .Distinct()
                .ToList();


        if (selectedCastMemberIds.Count > 0)
        {
            var selectedCastMembers =
                await _context.CastMembers
                    .Where(x =>
                        selectedCastMemberIds
                            .Contains(x.Id))
                    .ToListAsync();

            foreach (var castMember
                     in selectedCastMembers)
            {
                tvShow.CastMembers.Add(
                    castMember);
            }
        }


        await _context.SaveChangesAsync();


        // ------------------------------------------------
        // بروزرسانی نقش عوامل
        // ------------------------------------------------

        await UpdateCastMemberRolesAsync(
            tvShow.Id,
            dto.SelectedCastMembers);


        return true;
    }


    // ==================================================
    // دریافت جزئیات TVShow
    // ==================================================

    public async Task<TVShowDetailsDto?> GetDetailsAsync(
        int id)
    {
        var tvShow = await _context.TVShows
            .AsNoTracking()
            .Include(x => x.Genres)
            .Include(x => x.Languages)
            .Include(x => x.CastMembers)
            .FirstOrDefaultAsync(x => x.Id == id);


        if (tvShow == null)
            return null;


        var castMembers =
            await GetTVShowCastMemberDetailsAsync(
                tvShow.Id);


        return new TVShowDetailsDto
        {
            Id = tvShow.Id,

            Title = tvShow.Title,

            PosterPath =
                tvShow.PosterPath,

            ShortDescription =
                tvShow.ShortDescription,

            DownloadLink =
                tvShow.DownloadLink,

            DurationMinutes =
                tvShow.DurationMinutes,

            Year =
                tvShow.Year,

            ImdbRating =
                tvShow.ImdbRating,

            ViewCount =
                tvShow.ViewCount,

            Genres =
                tvShow.Genres
                    .Select(x => x.Name)
                    .ToList(),

            Languages =
                tvShow.Languages
                    .Select(x => x.Name)
                    .ToList(),

            AvailableQualities =
                tvShow.AvailableQualities
                    .ToList(),

            CastMembers =
                castMembers,

            EpisodeCount =
                await _context.TVShowEpisodes
                    .CountAsync(x =>
                        x.TVShowId == tvShow.Id),

            IsActive =
                tvShow.IsActive,

            IsArchived =
                tvShow.IsArchived,

            CreatedAt =
                tvShow.CreatedAt,

            UpdatedAt =
                tvShow.UpdatedAt,

            ArchivedAt =
                tvShow.ArchivedAt
        };
    }


    // ==================================================
    // دریافت TVShow های آرشیو شده
    // ==================================================

    public async Task<List<TVShowListDto>>
        GetArchivedAsync()
    {
        return await _context.TVShows
            .AsNoTracking()
            .Where(x => x.IsArchived)
            .OrderByDescending(x => x.ArchivedAt)
            .Select(x => new TVShowListDto
            {
                Id = x.Id,
                Title = x.Title,
                PosterPath = x.PosterPath,
                Year = x.Year,
                DurationMinutes = x.DurationMinutes,
                ImdbRating = x.ImdbRating,
                ViewCount = x.ViewCount,
                EpisodeCount = x.Episodes.Count,
                IsActive = x.IsActive,
                IsArchived = x.IsArchived,
                CreatedAt = x.CreatedAt,
                ArchivedAt = x.ArchivedAt
            })
            .ToListAsync();
    }


    // ==================================================
    // دریافت TVShow های حذف شده
    // ==================================================

    public async Task<List<TVShowListDto>>
        GetDeletedAsync()
    {
        return await _context.TVShows
            .IgnoreQueryFilters()
            .AsNoTracking()
            .Where(x => x.IsDeleted)
            .OrderByDescending(x => x.DeletedAt)
            .Select(x => new TVShowListDto
            {
                Id = x.Id,
                Title = x.Title,
                PosterPath = x.PosterPath,
                Year = x.Year,
                DurationMinutes = x.DurationMinutes,
                ImdbRating = x.ImdbRating,
                ViewCount = x.ViewCount,
                EpisodeCount = x.Episodes.Count,
                IsActive = x.IsActive,
                IsArchived = x.IsArchived,
                CreatedAt = x.CreatedAt,
                ArchivedAt = x.ArchivedAt
            })
            .ToListAsync();
    }


    // ==================================================
    // حذف TVShow
    // ==================================================

    public async Task<bool> DeleteAsync(int id)
    {
        var tvShow = await _context.TVShows
            .FirstOrDefaultAsync(x => x.Id == id);

        if (tvShow == null)
            return false;

        _context.TVShows.Remove(tvShow);

        await _context.SaveChangesAsync();

        return true;
    }


    // ==================================================
    // بازیابی TVShow
    // ==================================================

    public async Task<bool> RestoreAsync(int id)
    {
        var tvShow = await _context.TVShows
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(x =>
                x.Id == id &&
                x.IsDeleted);

        if (tvShow == null)
            return false;

        tvShow.IsDeleted = false;
        tvShow.DeletedAt = null;

        await _context.SaveChangesAsync();

        return true;
    }


    // ==================================================
    // فعال / غیرفعال کردن TVShow
    // ==================================================

    public async Task<bool> ChangeStatusAsync(int id)
    {
        var tvShow = await _context.TVShows
            .FirstOrDefaultAsync(x => x.Id == id);

        if (tvShow == null)
            return false;

        tvShow.IsActive =
            !tvShow.IsActive;

        await _context.SaveChangesAsync();

        return true;
    }


    // ==================================================
    // آرشیو TVShow
    // ==================================================

    public async Task<bool> ArchiveAsync(int id)
    {
        var tvShow = await _context.TVShows
            .FirstOrDefaultAsync(x => x.Id == id);

        if (tvShow == null)
            return false;

        tvShow.IsArchived = true;
        tvShow.ArchivedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }


    // ==================================================
    // خارج کردن از آرشیو
    // ==================================================

    public async Task<bool> UnarchiveAsync(int id)
    {
        var tvShow = await _context.TVShows
            .FirstOrDefaultAsync(x => x.Id == id);

        if (tvShow == null)
            return false;

        tvShow.IsArchived = false;
        tvShow.ArchivedAt = null;

        await _context.SaveChangesAsync();

        return true;
    }


    // ==================================================
    // دریافت نقش عوامل TVShow
    // ==================================================

    private async Task<List<MovieCastMemberDto>>
        GetTVShowCastMembersAsync(
            int tvShowId)
    {
        return await _context
            .Set<Dictionary<string, object>>(
                "MovieCastMembers")
            .Where(x =>
                EF.Property<int>(
                    x,
                    "MovieId") == tvShowId)
            .Select(x => new MovieCastMemberDto
            {
                CastMemberId =
                    EF.Property<int>(
                        x,
                        "CastMemberId"),

                CastType =
                    EF.Property<CastType>(
                        x,
                        "CastType")
            })
            .ToListAsync();
    }


    // ==================================================
    // دریافت جزئیات عوامل TVShow
    // ==================================================

    private async Task<List<MovieCastMemberDetailsDto>>
        GetTVShowCastMemberDetailsAsync(
            int tvShowId)
    {
        return await _context
            .Set<Dictionary<string, object>>(
                "MovieCastMembers")
            .Where(x =>
                EF.Property<int>(
                    x,
                    "MovieId") == tvShowId)
            .Join(
                _context.CastMembers,
                join => EF.Property<int>(
                    join,
                    "CastMemberId"),
                cast => cast.Id,
                (join, cast) => new
                {
                    CastMember = cast,

                    CastType =
                        EF.Property<CastType>(
                            join,
                            "CastType")
                })
            .Select(x => new MovieCastMemberDetailsDto
            {
                Id = x.CastMember.Id,
                FullName = x.CastMember.FullName,
                PhotoPath = x.CastMember.PhotoPath,
                CastType = x.CastType
            })
            .ToListAsync();
    }


    // ==================================================
    // ثبت نقش عوامل
    // ==================================================

    private async Task SaveCastMemberRolesAsync(
        int tvShowId,
        List<MovieCastMemberDto>
            selectedCastMembers)
    {
        foreach (var castMember
                 in selectedCastMembers)
        {
            var castType =
                castMember.CastType;

            var exists =
                await _context
                    .Set<Dictionary<string, object>>(
                        "MovieCastMembers")
                    .AnyAsync(x =>
                        EF.Property<int>(
                            x,
                            "MovieId") == tvShowId &&
                        EF.Property<int>(
                            x,
                            "CastMemberId")
                            == castMember.CastMemberId);

            if (exists)
                continue;

            _context
                .Set<Dictionary<string, object>>(
                    "MovieCastMembers")
                .Add(
                    new Dictionary<string, object>
                    {
                        ["MovieId"] = tvShowId,
                        ["CastMemberId"] =
                            castMember.CastMemberId,
                        ["CastType"] = castType
                    });
        }

        await _context.SaveChangesAsync();
    }


    // ==================================================
    // بروزرسانی نقش عوامل
    // ==================================================

    private async Task UpdateCastMemberRolesAsync(
        int tvShowId,
        List<MovieCastMemberDto>
            selectedCastMembers)
    {
        var existingRoles =
            await _context
                .Set<Dictionary<string, object>>(
                    "MovieCastMembers")
                .Where(x =>
                    EF.Property<int>(
                        x,
                        "MovieId") == tvShowId)
                .ToListAsync();


        foreach (var role in existingRoles)
        {
            _context
                .Set<Dictionary<string, object>>(
                    "MovieCastMembers")
                .Remove(role);
        }


        foreach (var castMember
                 in selectedCastMembers)
        {
            _context
                .Set<Dictionary<string, object>>(
                    "MovieCastMembers")
                .Add(
                    new Dictionary<string, object>
                    {
                        ["MovieId"] = tvShowId,
                        ["CastMemberId"] =
                            castMember.CastMemberId,
                        ["CastType"] =
                            castMember.CastType
                    });
        }


        await _context.SaveChangesAsync();
    }
}