using DataBase.Context;
using DataBase.Enum;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Service.Extention.ImageExtention;

namespace Service.Admin.TvSeries.Series;

// ======================================================
// Interface - Series
// ======================================================
public interface ISeriesService
{
    // دریافت لیست سریال‌ها
    Task<List<SeriesListDto>> GetAllAsync();

    // دریافت اطلاعات لازم برای فرم ایجاد سریال
    Task<SeriesCreateDto> GetCreateDataAsync();

    // تکمیل اطلاعات فرم Create
    Task FillCreateFormDataAsync(SeriesCreateDto dto);

    // ایجاد سریال جدید
    Task<int> CreateAsync(SeriesCreateDto dto);

    // دریافت اطلاعات سریال برای ویرایش
    Task<SeriesEditDto?> GetEditDataAsync(int id);

    // تکمیل اطلاعات فرم Edit
    Task FillEditFormDataAsync(SeriesEditDto dto);

    // ویرایش سریال
    Task<bool> UpdateAsync(SeriesEditDto dto);

    // دریافت جزئیات سریال
    Task<SeriesDetailsDto?> GetDetailsAsync(int id);

    // دریافت سریال‌های آرشیو شده
    Task<List<SeriesListDto>> GetArchivedAsync();

    // دریافت سریال‌های حذف شده
    Task<List<SeriesListDto>> GetDeletedAsync();

    // حذف نرم
    Task<bool> DeleteAsync(int id);

    // بازیابی سریال حذف‌شده
    Task<bool> RestoreAsync(int id);

    // فعال / غیرفعال کردن سریال
    Task<bool> ChangeStatusAsync(int id);

    // آرشیو سریال
    Task<bool> ArchiveAsync(int id);

    // خارج کردن از آرشیو
    Task<bool> UnarchiveAsync(int id);
}


// ======================================================
// Service - Series
// ======================================================

public class SeriesService : ISeriesService
{
    private readonly MyContext _context;

    private readonly IWebHostEnvironment _webHostEnvironment;

    public SeriesService(
        MyContext context,
        IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }


    // ==================================================
    // دریافت تمام سریال‌ها
    // ==================================================

    public async Task<List<SeriesListDto>> GetAllAsync()
    {
        return await _context.Series
            .AsNoTracking()
            .Where(x => !x.IsArchived)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new SeriesListDto
            {
                Id = x.Id,
                Title = x.Title,
                PosterPath = x.PosterPath,
                Year = x.Year,
                DurationMinutes = x.DurationMinutes,
                ImdbRating = x.ImdbRating,
                ViewCount = x.ViewCount,
                SeasonCount = x.Seasons.Count,
                IsActive = x.IsActive,
                IsArchived = x.IsArchived,
                CreatedAt = x.CreatedAt
            })
            .ToListAsync();
    }


    // ==================================================
    // دریافت اطلاعات اولیه فرم Create
    // ==================================================

    public async Task<SeriesCreateDto> GetCreateDataAsync()
    {
        var dto = new SeriesCreateDto();

        await FillCreateFormDataAsync(dto);

        return dto;
    }


    // ==================================================
    // پر کردن اطلاعات فرم Create
    // ==================================================

    public async Task FillCreateFormDataAsync(SeriesCreateDto dto)
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
            .Select(x => new SeriesSelectionItemDto
            {
                Id = x.Id,
                Name = x.Name,
                IsSelected = dto.SelectedGenreIds.Contains(x.Id)
            })
            .ToList();


        dto.Languages = languages
            .Select(x => new SeriesSelectionItemDto
            {
                Id = x.Id,
                Name = x.Name,
                IsSelected = dto.SelectedLanguageIds.Contains(x.Id)
            })
            .ToList();


        dto.CastMembers = castMembers
            .Select(x =>
            {
                var selected = dto.SelectedCastMembers
                    .FirstOrDefault(c => c.CastMemberId == x.Id);

                return new SeriesCastMemberSelectionDto
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    PhotoPath = x.PhotoPath,
                    IsSelected = selected != null,
                    CastType = selected?.CastType ?? CastType.Actor
                };
            })
            .ToList();
    }


    // ==================================================
    // ایجاد سریال
    // ==================================================

    public async Task<int> CreateAsync(SeriesCreateDto dto)
    {
        string posterPath = string.Empty;


        // ------------------------------------------------
        // ذخیره پوستر
        // ------------------------------------------------

        if (dto.Poster != null)
        {
            posterPath = await dto.Poster.SaveImageAsync(
                _webHostEnvironment.WebRootPath,
                "movies",
                "posters");
        }


        // ------------------------------------------------
        // ایجاد Series
        // ------------------------------------------------

        var series = new DataBase.Entity.Series
        {
            Title = dto.Title.Trim(),

            PosterPath = posterPath,

            ShortDescription = dto.ShortDescription.Trim(),

            DownloadLink = string.IsNullOrWhiteSpace(dto.DownloadLink)
                ? null
                : dto.DownloadLink.Trim(),

            DurationMinutes = dto.DurationMinutes,

            Year = dto.Year,

            ImdbRating = dto.ImdbRating,

            ViewCount = dto.ViewCount,

            AvailableQualities = dto.AvailableQualities
                .Distinct()
                .ToList()
        };


        // ------------------------------------------------
        // دریافت ژانرهای انتخاب شده
        // ------------------------------------------------

        if (dto.SelectedGenreIds.Count > 0)
        {
            series.Genres = await _context.Genres
                .Where(x => dto.SelectedGenreIds.Contains(x.Id))
                .ToListAsync();
        }


        // ------------------------------------------------
        // دریافت زبان‌های انتخاب شده
        // ------------------------------------------------

        if (dto.SelectedLanguageIds.Count > 0)
        {
            series.Languages = await _context.Languages
                .Where(x => dto.SelectedLanguageIds.Contains(x.Id))
                .ToListAsync();
        }


        // ------------------------------------------------
        // دریافت عوامل انتخاب شده
        // ------------------------------------------------

        var selectedCastMemberIds = dto.SelectedCastMembers
            .Select(x => x.CastMemberId)
            .Distinct()
            .ToList();

        if (selectedCastMemberIds.Count > 0)
        {
            series.CastMembers = await _context.CastMembers
                .Where(x => selectedCastMemberIds.Contains(x.Id))
                .ToListAsync();
        }


        // ------------------------------------------------
        // ثبت Series
        // ------------------------------------------------

        _context.Series.Add(series);

        await _context.SaveChangesAsync();


        // ------------------------------------------------
        // ثبت نقش عوامل
        // ------------------------------------------------

        await SaveCastMemberRolesAsync(
            series.Id,
            dto.SelectedCastMembers);


        return series.Id;
    }


    // ==================================================
    // دریافت اطلاعات سریال برای Edit
    // ==================================================

    public async Task<SeriesEditDto?> GetEditDataAsync(int id)
    {
        var series = await _context.Series
            .Include(x => x.Genres)
            .Include(x => x.Languages)
            .Include(x => x.CastMembers)
            .FirstOrDefaultAsync(x => x.Id == id);


        if (series == null)
            return null;


        var selectedCastMembers =
            await GetSeriesCastMembersAsync(series.Id);


        var dto = new SeriesEditDto
        {
            Id = series.Id,

            Title = series.Title,

            ExistingPosterPath = series.PosterPath,

            ShortDescription = series.ShortDescription,

            DownloadLink = series.DownloadLink,

            DurationMinutes = series.DurationMinutes,

            Year = series.Year,

            ImdbRating = series.ImdbRating,

            ViewCount = series.ViewCount,

            SelectedGenreIds = series.Genres
                .Select(x => x.Id)
                .ToList(),

            SelectedLanguageIds = series.Languages
                .Select(x => x.Id)
                .ToList(),

            SelectedCastMembers = selectedCastMembers,

            AvailableQualities = series.AvailableQualities
                .ToList()
        };


        await FillEditFormDataAsync(dto);

        return dto;
    }


    // ==================================================
    // پر کردن اطلاعات فرم Edit
    // ==================================================

    public async Task FillEditFormDataAsync(SeriesEditDto dto)
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
            .Select(x => new SeriesSelectionItemDto
            {
                Id = x.Id,
                Name = x.Name,
                IsSelected = dto.SelectedGenreIds.Contains(x.Id)
            })
            .ToList();


        dto.Languages = languages
            .Select(x => new SeriesSelectionItemDto
            {
                Id = x.Id,
                Name = x.Name,
                IsSelected = dto.SelectedLanguageIds.Contains(x.Id)
            })
            .ToList();


        dto.CastMembers = castMembers
            .Select(x =>
            {
                var selected = dto.SelectedCastMembers
                    .FirstOrDefault(c => c.CastMemberId == x.Id);

                return new SeriesCastMemberSelectionDto
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    PhotoPath = x.PhotoPath,
                    IsSelected = selected != null,
                    CastType = selected?.CastType ?? CastType.Actor
                };
            })
            .ToList();
    }


    // ==================================================
    // ویرایش سریال
    // ==================================================

    public async Task<bool> UpdateAsync(SeriesEditDto dto)
    {
        var series = await _context.Series
            .Include(x => x.Genres)
            .Include(x => x.Languages)
            .Include(x => x.CastMembers)
            .FirstOrDefaultAsync(x => x.Id == dto.Id);


        if (series == null)
            return false;


        // ------------------------------------------------
        // بروزرسانی اطلاعات اصلی
        // ------------------------------------------------

        series.Title = dto.Title.Trim();

        series.ShortDescription = dto.ShortDescription.Trim();

        series.DownloadLink = string.IsNullOrWhiteSpace(dto.DownloadLink)
            ? null
            : dto.DownloadLink.Trim();

        series.DurationMinutes = dto.DurationMinutes;

        series.Year = dto.Year;

        series.ImdbRating = dto.ImdbRating;

        series.ViewCount = dto.ViewCount;

        series.AvailableQualities = dto.AvailableQualities
            .Distinct()
            .ToList();


        // ------------------------------------------------
        // اگر پوستر جدید ارسال شده باشد
        // ------------------------------------------------

        if (dto.Poster != null)
        {
            var newPosterPath = await dto.Poster.SaveImageAsync(
                _webHostEnvironment.WebRootPath,
                "movies",
                "posters");


            if (!string.IsNullOrWhiteSpace(series.PosterPath))
            {
                var oldFilePath = Path.Combine(
                    _webHostEnvironment.WebRootPath,
                    series.PosterPath.TrimStart('/', '\\'));


                if (File.Exists(oldFilePath))
                {
                    File.Delete(oldFilePath);
                }
            }


            series.PosterPath = newPosterPath;
        }


        // ------------------------------------------------
        // بروزرسانی ژانرها
        // ------------------------------------------------

        series.Genres.Clear();

        if (dto.SelectedGenreIds.Count > 0)
        {
            var selectedGenres = await _context.Genres
                .Where(x => dto.SelectedGenreIds.Contains(x.Id))
                .ToListAsync();

            foreach (var genre in selectedGenres)
            {
                series.Genres.Add(genre);
            }
        }


        // ------------------------------------------------
        // بروزرسانی زبان‌ها
        // ------------------------------------------------

        series.Languages.Clear();

        if (dto.SelectedLanguageIds.Count > 0)
        {
            var selectedLanguages = await _context.Languages
                .Where(x => dto.SelectedLanguageIds.Contains(x.Id))
                .ToListAsync();

            foreach (var language in selectedLanguages)
            {
                series.Languages.Add(language);
            }
        }


        // ------------------------------------------------
        // بروزرسانی عوامل سریال
        // ------------------------------------------------

        series.CastMembers.Clear();

        var selectedCastMemberIds = dto.SelectedCastMembers
            .Select(x => x.CastMemberId)
            .Distinct()
            .ToList();


        if (selectedCastMemberIds.Count > 0)
        {
            var selectedCastMembers = await _context.CastMembers
                .Where(x => selectedCastMemberIds.Contains(x.Id))
                .ToListAsync();

            foreach (var castMember in selectedCastMembers)
            {
                series.CastMembers.Add(castMember);
            }
        }


        await _context.SaveChangesAsync();


        // ------------------------------------------------
        // بروزرسانی نقش عوامل
        // ------------------------------------------------

        await UpdateCastMemberRolesAsync(
            series.Id,
            dto.SelectedCastMembers);


        return true;
    }


    // ==================================================
    // دریافت جزئیات سریال
    // ==================================================

    public async Task<SeriesDetailsDto?> GetDetailsAsync(int id)
    {
        var series = await _context.Series
            .AsNoTracking()
            .Include(x => x.Genres)
            .Include(x => x.Languages)
            .Include(x => x.CastMembers)
            .Include(x => x.Seasons)
                .ThenInclude(x => x.Episodes)
            .FirstOrDefaultAsync(x => x.Id == id);


        if (series == null)
            return null;


        var castMembers =
            await GetSeriesCastMemberDetailsAsync(series.Id);


        return new SeriesDetailsDto
        {
            Id = series.Id,

            Title = series.Title,

            PosterPath = series.PosterPath,

            ShortDescription = series.ShortDescription,

            DownloadLink = series.DownloadLink,

            DurationMinutes = series.DurationMinutes,

            Year = series.Year,

            ImdbRating = series.ImdbRating,

            ViewCount = series.ViewCount,

            Genres = series.Genres
                .Select(x => x.Name)
                .ToList(),

            Languages = series.Languages
                .Select(x => x.Name)
                .ToList(),

            AvailableQualities = series.AvailableQualities
                .ToList(),

            CastMembers = castMembers,

            Seasons = series.Seasons
                .OrderBy(x => x.Id)
                .Select(x => new SeriesSeasonItemDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    DownloadLink = x.DownloadLink,
                    EpisodeCount = x.Episodes.Count
                })
                .ToList(),

            IsActive = series.IsActive,

            IsArchived = series.IsArchived,

            CreatedAt = series.CreatedAt,

            UpdatedAt = series.UpdatedAt
        };
    }


    // ==================================================
    // دریافت نقش عوامل یک سریال
    // ==================================================

    private async Task<List<SeriesCastMemberDto>>
        GetSeriesCastMembersAsync(int seriesId)
    {
        return await _context
            .Set<Dictionary<string, object>>("MovieCastMembers")
            .Where(x =>
                EF.Property<int>(x, "MovieId") == seriesId)
            .Select(x => new SeriesCastMemberDto
            {
                CastMemberId =
                    EF.Property<int>(x, "CastMemberId"),

                CastType =
                    EF.Property<CastType>(x, "CastType")
            })
            .ToListAsync();
    }


    // ==================================================
    // دریافت جزئیات عوامل یک سریال
    // ==================================================

    private async Task<List<SeriesCastMemberDetailsDto>>
        GetSeriesCastMemberDetailsAsync(int seriesId)
    {
        return await _context.CastMembers
            .AsNoTracking()
            .Where(x =>
                x.Movies.Any(m => m.Id == seriesId))
            .Select(x => new SeriesCastMemberDetailsDto
            {
                Id = x.Id,

                FullName = x.FullName,

                PhotoPath = x.PhotoPath,

                CastType = _context
                    .Set<Dictionary<string, object>>(
                        "MovieCastMembers")
                    .Where(j =>
                        EF.Property<int>(
                            j,
                            "MovieId") == seriesId &&
                        EF.Property<int>(
                            j,
                            "CastMemberId") == x.Id)
                    .Select(j =>
                        EF.Property<CastType>(
                            j,
                            "CastType"))
                    .First()
            })
            .ToListAsync();
    }


    // ==================================================
    // ثبت نقش عوامل
    // ==================================================

    private async Task SaveCastMemberRolesAsync(
        int seriesId,
        List<SeriesCastMemberDto> castMembers)
    {
        if (castMembers.Count == 0)
            return;


        var joinEntities =
            _context.Set<Dictionary<string, object>>(
                "MovieCastMembers");


        foreach (var item in castMembers
            .GroupBy(x => x.CastMemberId)
            .Select(x => x.First()))
        {
            joinEntities.Add(
                new Dictionary<string, object>
                {
                    ["MovieId"] = seriesId,

                    ["CastMemberId"] = item.CastMemberId,

                    ["CastType"] = item.CastType
                });
        }


        await _context.SaveChangesAsync();
    }


    // ==================================================
    // بروزرسانی نقش عوامل
    // ==================================================

    private async Task UpdateCastMemberRolesAsync(
        int seriesId,
        List<SeriesCastMemberDto> castMembers)
    {
        var joinEntities =
            _context.Set<Dictionary<string, object>>(
                "MovieCastMembers");


        var existingEntities =
            await joinEntities
                .Where(x =>
                    EF.Property<int>(
                        x,
                        "MovieId") == seriesId)
                .ToListAsync();


        foreach (var entity in existingEntities)
        {
            joinEntities.Remove(entity);
        }


        foreach (var item in castMembers
            .GroupBy(x => x.CastMemberId)
            .Select(x => x.First()))
        {
            joinEntities.Add(
                new Dictionary<string, object>
                {
                    ["MovieId"] = seriesId,

                    ["CastMemberId"] = item.CastMemberId,

                    ["CastType"] = item.CastType
                });
        }


        await _context.SaveChangesAsync();
    }


    // ==================================================
    // دریافت سریال‌های آرشیو شده
    // ==================================================

    public async Task<List<SeriesListDto>> GetArchivedAsync()
    {
        return await _context.Series
            .IgnoreQueryFilters()
            .Where(x =>
                x.IsArchived &&
                !x.IsDeleted)
            .Select(series => new SeriesListDto
            {
                Id = series.Id,

                Title = series.Title,

                PosterPath = series.PosterPath,

                Year = series.Year,

                ImdbRating = series.ImdbRating,

                DurationMinutes = series.DurationMinutes,

                ViewCount = series.ViewCount,

                SeasonCount = series.Seasons.Count,

                IsActive = series.IsActive,

                IsArchived = series.IsArchived,

                CreatedAt = series.CreatedAt,

                ArchivedAt = series.ArchivedAt
            })
            .ToListAsync();
    }


    // ==================================================
    // دریافت سریال‌های حذف شده
    // ==================================================

    public async Task<List<SeriesListDto>> GetDeletedAsync()
    {
        return await _context.Series
            .IgnoreQueryFilters()
            .AsNoTracking()
            .Where(x => x.IsDeleted)
            .OrderByDescending(x => x.DeletedAt)
            .Select(x => new SeriesListDto
            {
                Id = x.Id,

                Title = x.Title,

                PosterPath = x.PosterPath,

                Year = x.Year,

                ImdbRating = x.ImdbRating,

                DurationMinutes = x.DurationMinutes,

                ViewCount = x.ViewCount,

                SeasonCount = x.Seasons.Count,

                IsActive = x.IsActive,

                IsArchived = x.IsArchived,

                CreatedAt = x.CreatedAt
            })
            .ToListAsync();
    }


    // ==================================================
    // حذف نرم
    // ==================================================

    public async Task<bool> DeleteAsync(int id)
    {
        var series = await _context.Series
            .FirstOrDefaultAsync(x => x.Id == id);


        if (series == null)
            return false;


        _context.Series.Remove(series);

        await _context.SaveChangesAsync();

        return true;
    }


    // ==================================================
    // Restore سریال حذف شده
    // ==================================================

    public async Task<bool> RestoreAsync(int id)
    {
        var series = await _context.Series
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(x =>
                x.Id == id &&
                x.IsDeleted);


        if (series == null)
            return false;


        series.IsDeleted = false;

        series.DeletedAt = null;


        await _context.SaveChangesAsync();

        return true;
    }


    // ==================================================
    // فعال / غیرفعال کردن
    // ==================================================

    public async Task<bool> ChangeStatusAsync(int id)
    {
        var series = await _context.Series
            .FirstOrDefaultAsync(x => x.Id == id);


        if (series == null)
            return false;


        series.IsActive = !series.IsActive;


        await _context.SaveChangesAsync();

        return true;
    }


    // ==================================================
    // آرشیو کردن
    // ==================================================

    public async Task<bool> ArchiveAsync(int id)
    {
        var series = await _context.Series
            .FirstOrDefaultAsync(x => x.Id == id);


        if (series == null)
            return false;


        series.IsArchived = true;

        series.ArchivedAt = DateTime.UtcNow;


        await _context.SaveChangesAsync();

        return true;
    }


    // ==================================================
    // خارج کردن از آرشیو
    // ==================================================

    public async Task<bool> UnarchiveAsync(int id)
    {
        var series = await _context.Series
            .FirstOrDefaultAsync(x => x.Id == id);


        if (series == null)
            return false;


        series.IsArchived = false;

        series.ArchivedAt = null;


        await _context.SaveChangesAsync();

        return true;
    }
}