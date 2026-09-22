using DataBase.Context;
using DataBase.Entity;
using DataBase.Enum;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Service.Extention.ImageExtention;
// ======================================================
// Interface
// ======================================================

public interface IMovieService
{
    // دریافت لیست فیلم‌ها
    Task<List<MovieListDto>> GetAllAsync();

    // دریافت اطلاعات لازم برای فرم ایجاد فیلم
    Task<MovieCreateDto> GetCreateDataAsync();

    // تکمیل اطلاعات فرم Create
    Task FillCreateFormDataAsync(MovieCreateDto dto);

    // ایجاد فیلم جدید
    Task<int> CreateAsync(MovieCreateDto dto);

    // دریافت اطلاعات فیلم برای ویرایش
    Task<MovieEditDto?> GetEditDataAsync(int id);

    // تکمیل اطلاعات فرم Edit
    Task FillEditFormDataAsync(MovieEditDto dto);

    // ویرایش فیلم
    Task<bool> UpdateAsync(MovieEditDto dto);

    // دریافت جزئیات فیلم
    Task<MovieDetailsDto?> GetDetailsAsync(int id);

    // دریافت فیلم‌های آرشیو شده
    Task<List<MovieListDto>> GetArchivedAsync();

    // دریافت فیلم‌های حذف شده
    Task<List<MovieListDto>> GetDeletedAsync();

    // حذف نرم
    Task<bool> DeleteAsync(int id);

    // بازیابی فیلم حذف‌شده
    Task<bool> RestoreAsync(int id);

    // فعال / غیرفعال کردن فیلم
    Task<bool> ChangeStatusAsync(int id);

    // آرشیو فیلم
    Task<bool> ArchiveAsync(int id);

    // خارج کردن از آرشیو
    Task<bool> UnarchiveAsync(int id);
}



// ======================================================
// Service
// ======================================================

public class MovieService : IMovieService
{
    private readonly MyContext _context;

    private readonly IWebHostEnvironment _webHostEnvironment;

    public MovieService(
        MyContext context,
        IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }


    // ==================================================
    // دریافت تمام فیلم‌ها
    // ==================================================

    public async Task<List<MovieListDto>> GetAllAsync()
    {
        return await _context.Movies
            .AsNoTracking()
            .Where(x => !x.IsArchived)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new MovieListDto
            {
                Id = x.Id,
                Title = x.Title,
                PosterPath = x.PosterPath,
                Year = x.Year,
                DurationMinutes = x.DurationMinutes,
                ImdbRating = x.ImdbRating,
                ViewCount = x.ViewCount,
                IsActive = x.IsActive,
                IsArchived = x.IsArchived,
                CreatedAt = x.CreatedAt
            })
            .ToListAsync();
    }


    // ==================================================
    // دریافت اطلاعات اولیه فرم Create
    // ==================================================

    public async Task<MovieCreateDto> GetCreateDataAsync()
    {
        var dto = new MovieCreateDto();

        await FillCreateFormDataAsync(dto);

        return dto;
    }


    // ==================================================
    // پر کردن اطلاعات فرم Create
    // ==================================================

    public async Task FillCreateFormDataAsync(MovieCreateDto dto)
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
                IsSelected = dto.SelectedGenreIds.Contains(x.Id)
            })
            .ToList();


        dto.Languages = languages
            .Select(x => new MovieSelectionItemDto
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

                return new MovieCastMemberSelectionDto
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
    // ایجاد فیلم
    // ==================================================

    public async Task<int> CreateAsync(MovieCreateDto dto)
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
        // ایجاد Movie
        // ------------------------------------------------

        var movie = new Movie
        {
            Title = dto.Title.Trim(),

            PosterPath = posterPath,

            ShortDescription = dto.ShortDescription.Trim(),

            DurationMinutes = dto.DurationMinutes,

            Year = dto.Year,

            ImdbRating = dto.ImdbRating,

            ViewCount = dto.ViewCount,

            // لینک دانلود - اختیاری
            DownloadLink = string.IsNullOrWhiteSpace(dto.DownloadLink)
                ? null
                : dto.DownloadLink.Trim(),

            AvailableQualities = dto.AvailableQualities
                .Distinct()
                .ToList()
        };


        // ------------------------------------------------
        // دریافت ژانرهای انتخاب شده
        // ------------------------------------------------

        if (dto.SelectedGenreIds.Count > 0)
        {
            movie.Genres = await _context.Genres
                .Where(x => dto.SelectedGenreIds.Contains(x.Id))
                .ToListAsync();
        }


        // ------------------------------------------------
        // دریافت زبان‌های انتخاب شده
        // ------------------------------------------------

        if (dto.SelectedLanguageIds.Count > 0)
        {
            movie.Languages = await _context.Languages
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
            movie.CastMembers = await _context.CastMembers
                .Where(x => selectedCastMemberIds.Contains(x.Id))
                .ToListAsync();
        }


        // ------------------------------------------------
        // ثبت Movie
        // ------------------------------------------------

        _context.Movies.Add(movie);

        await _context.SaveChangesAsync();


        // ------------------------------------------------
        // ثبت نقش عوامل در جدول واسط EF
        // ------------------------------------------------

        await SaveCastMemberRolesAsync(
            movie.Id,
            dto.SelectedCastMembers);


        return movie.Id;
    }


    // ==================================================
    // دریافت اطلاعات فیلم برای Edit
    // ==================================================

    public async Task<MovieEditDto?> GetEditDataAsync(int id)
    {
        var movie = await _context.Movies
            .Include(x => x.Genres)
            .Include(x => x.Languages)
            .Include(x => x.CastMembers)
            .FirstOrDefaultAsync(x => x.Id == id);


        if (movie == null)
            return null;


        var selectedCastMembers =
            await GetMovieCastMembersAsync(movie.Id);


        var dto = new MovieEditDto
        {
            Id = movie.Id,

            Title = movie.Title,

            ExistingPosterPath = movie.PosterPath,

            ShortDescription = movie.ShortDescription,

            DurationMinutes = movie.DurationMinutes,

            Year = movie.Year,

            ImdbRating = movie.ImdbRating,

            ViewCount = movie.ViewCount,

            // لینک دانلود
            DownloadLink = movie.DownloadLink,

            SelectedGenreIds = movie.Genres
                .Select(x => x.Id)
                .ToList(),

            SelectedLanguageIds = movie.Languages
                .Select(x => x.Id)
                .ToList(),

            SelectedCastMembers = selectedCastMembers,

            AvailableQualities = movie.AvailableQualities
                .ToList()
        };


        await FillEditFormDataAsync(dto);

        return dto;
    }


    // ==================================================
    // پر کردن اطلاعات فرم Edit
    // ==================================================

    public async Task FillEditFormDataAsync(MovieEditDto dto)
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
                IsSelected = dto.SelectedGenreIds.Contains(x.Id)
            })
            .ToList();


        dto.Languages = languages
            .Select(x => new MovieSelectionItemDto
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

                return new MovieCastMemberSelectionDto
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
    // ویرایش فیلم
    // ==================================================

    public async Task<bool> UpdateAsync(MovieEditDto dto)
    {
        var movie = await _context.Movies
            .Include(x => x.Genres)
            .Include(x => x.Languages)
            .Include(x => x.CastMembers)
            .FirstOrDefaultAsync(x => x.Id == dto.Id);


        if (movie == null)
            return false;


        // ------------------------------------------------
        // بروزرسانی اطلاعات اصلی
        // ------------------------------------------------

        movie.Title = dto.Title.Trim();

        movie.ShortDescription = dto.ShortDescription.Trim();

        movie.DurationMinutes = dto.DurationMinutes;

        movie.Year = dto.Year;

        movie.ImdbRating = dto.ImdbRating;

        movie.ViewCount = dto.ViewCount;

        // لینک دانلود - اختیاری
        movie.DownloadLink = string.IsNullOrWhiteSpace(dto.DownloadLink)
            ? null
            : dto.DownloadLink.Trim();

        movie.AvailableQualities = dto.AvailableQualities
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


            if (!string.IsNullOrWhiteSpace(movie.PosterPath))
            {
                var oldFilePath = Path.Combine(
                    _webHostEnvironment.WebRootPath,
                    movie.PosterPath.TrimStart('/', '\\'));


                if (File.Exists(oldFilePath))
                {
                    File.Delete(oldFilePath);
                }
            }


            movie.PosterPath = newPosterPath;
        }


        // ------------------------------------------------
        // بروزرسانی ژانرها
        // ------------------------------------------------

        movie.Genres.Clear();


        if (dto.SelectedGenreIds.Count > 0)
        {
            var selectedGenres = await _context.Genres
                .Where(x => dto.SelectedGenreIds.Contains(x.Id))
                .ToListAsync();


            foreach (var genre in selectedGenres)
            {
                movie.Genres.Add(genre);
            }
        }


        // ------------------------------------------------
        // بروزرسانی زبان‌ها
        // ------------------------------------------------

        movie.Languages.Clear();


        if (dto.SelectedLanguageIds.Count > 0)
        {
            var selectedLanguages = await _context.Languages
                .Where(x => dto.SelectedLanguageIds.Contains(x.Id))
                .ToListAsync();


            foreach (var language in selectedLanguages)
            {
                movie.Languages.Add(language);
            }
        }


        // ------------------------------------------------
        // بروزرسانی عوامل فیلم
        // ------------------------------------------------

        movie.CastMembers.Clear();


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
                movie.CastMembers.Add(castMember);
            }
        }


        await _context.SaveChangesAsync();


        // ------------------------------------------------
        // بروزرسانی نقش عوامل
        // ------------------------------------------------

        await UpdateCastMemberRolesAsync(
            movie.Id,
            dto.SelectedCastMembers);


        return true;
    }


    // ==================================================
    // دریافت جزئیات فیلم
    // ==================================================

    public async Task<MovieDetailsDto?> GetDetailsAsync(int id)
    {
        var movie = await _context.Movies
            .AsNoTracking()
            .Include(x => x.Genres)
            .Include(x => x.Languages)
            .Include(x => x.CastMembers)
            .FirstOrDefaultAsync(x => x.Id == id);


        if (movie == null)
            return null;


        var castMembers =
            await GetMovieCastMemberDetailsAsync(movie.Id);


        return new MovieDetailsDto
        {
            Id = movie.Id,

            Title = movie.Title,

            PosterPath = movie.PosterPath,

            ShortDescription = movie.ShortDescription,

            DurationMinutes = movie.DurationMinutes,

            Year = movie.Year,

            ImdbRating = movie.ImdbRating,

            ViewCount = movie.ViewCount,

            // لینک دانلود
            DownloadLink = movie.DownloadLink,

            Genres = movie.Genres
                .Select(x => x.Name)
                .ToList(),

            Languages = movie.Languages
                .Select(x => x.Name)
                .ToList(),

            AvailableQualities = movie.AvailableQualities
                .ToList(),

            CastMembers = castMembers,

            IsActive = movie.IsActive,

            IsArchived = movie.IsArchived,

            CreatedAt = movie.CreatedAt,

            UpdatedAt = movie.UpdatedAt
        };
    }


    // ==================================================
    // دریافت نقش عوامل یک فیلم
    // ==================================================

    private async Task<List<MovieCastMemberDto>>
        GetMovieCastMembersAsync(int movieId)
    {
        return await _context
            .Set<Dictionary<string, object>>("MovieCastMembers")
            .Where(x =>
                EF.Property<int>(x, "MovieId") == movieId)
            .Select(x => new MovieCastMemberDto
            {
                CastMemberId =
                    EF.Property<int>(x, "CastMemberId"),

                CastType =
                    EF.Property<CastType>(x, "CastType")
            })
            .ToListAsync();
    }


    // ==================================================
    // دریافت جزئیات عوامل یک فیلم
    // ==================================================

    private async Task<List<MovieCastMemberDetailsDto>>
        GetMovieCastMemberDetailsAsync(int movieId)
    {
        return await _context.CastMembers
            .AsNoTracking()
            .Where(x =>
                x.Movies.Any(m => m.Id == movieId))
            .Select(x => new MovieCastMemberDetailsDto
            {
                Id = x.Id,

                FullName = x.FullName,

                PhotoPath = x.PhotoPath,

                CastType = _context
                    .Set<Dictionary<string, object>>("MovieCastMembers")
                    .Where(j =>
                        EF.Property<int>(j, "MovieId") == movieId &&
                        EF.Property<int>(j, "CastMemberId") == x.Id)
                    .Select(j =>
                        EF.Property<CastType>(j, "CastType"))
                    .First()
            })
            .ToListAsync();
    }


    // ==================================================
    // ثبت نقش عوامل
    // ==================================================

    private async Task SaveCastMemberRolesAsync(
        int movieId,
        List<MovieCastMemberDto> castMembers)
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
                    ["MovieId"] = movieId,

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
        int movieId,
        List<MovieCastMemberDto> castMembers)
    {
        var joinEntities =
            _context.Set<Dictionary<string, object>>(
                "MovieCastMembers");


        var existingEntities =
            await joinEntities
                .Where(x =>
                    EF.Property<int>(x, "MovieId") == movieId)
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
                    ["MovieId"] = movieId,

                    ["CastMemberId"] = item.CastMemberId,

                    ["CastType"] = item.CastType
                });
        }


        await _context.SaveChangesAsync();
    }


    // ==================================================
    // دریافت فیلم‌های آرشیو شده
    // ==================================================

    public async Task<List<MovieListDto>> GetArchivedAsync()
    {
        return await _context.Movies
            .IgnoreQueryFilters()
            .Where(x => x.IsArchived && !x.IsDeleted)
            .Select(movie => new MovieListDto
            {
                Id = movie.Id,

                Title = movie.Title,

                PosterPath = movie.PosterPath,

                Year = movie.Year,

                ImdbRating = movie.ImdbRating,

                DurationMinutes = movie.DurationMinutes,

                ViewCount = movie.ViewCount,

                IsActive = movie.IsActive,

                IsArchived = movie.IsArchived,

                CreatedAt = movie.CreatedAt,

                ArchivedAt = movie.ArchivedAt
            })
            .ToListAsync();
    }


    // ==================================================
    // دریافت فیلم‌های حذف شده
    // ==================================================

    public async Task<List<MovieListDto>> GetDeletedAsync()
    {
        return await _context.Movies
            .IgnoreQueryFilters()
            .AsNoTracking()
            .Where(x => x.IsDeleted)
            .OrderByDescending(x => x.DeletedAt)
            .Select(x => new MovieListDto
            {
                Id = x.Id,

                Title = x.Title,

                PosterPath = x.PosterPath,

                Year = x.Year,

                DurationMinutes = x.DurationMinutes,

                ImdbRating = x.ImdbRating,

                ViewCount = x.ViewCount,

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
        var movie = await _context.Movies
            .FirstOrDefaultAsync(x => x.Id == id);


        if (movie == null)
            return false;


        _context.Movies.Remove(movie);

        await _context.SaveChangesAsync();

        return true;
    }


    // ==================================================
    // Restore فیلم حذف شده
    // ==================================================

    public async Task<bool> RestoreAsync(int id)
    {
        var movie = await _context.Movies
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(x =>
                x.Id == id &&
                x.IsDeleted);


        if (movie == null)
            return false;


        movie.IsDeleted = false;

        movie.DeletedAt = null;


        await _context.SaveChangesAsync();

        return true;
    }


    // ==================================================
    // فعال / غیرفعال کردن
    // ==================================================

    public async Task<bool> ChangeStatusAsync(int id)
    {
        var movie = await _context.Movies
            .FirstOrDefaultAsync(x => x.Id == id);


        if (movie == null)
            return false;


        movie.IsActive = !movie.IsActive;


        await _context.SaveChangesAsync();

        return true;
    }


    // ==================================================
    // آرشیو کردن
    // ==================================================

    public async Task<bool> ArchiveAsync(int id)
    {
        var movie = await _context.Movies
            .FirstOrDefaultAsync(x => x.Id == id);


        if (movie == null)
            return false;


        movie.IsArchived = true;

        movie.ArchivedAt = DateTime.UtcNow;


        await _context.SaveChangesAsync();

        return true;
    }


    // ==================================================
    // خارج کردن از آرشیو
    // ==================================================

    public async Task<bool> UnarchiveAsync(int id)
    {
        var movie = await _context.Movies
            .FirstOrDefaultAsync(x => x.Id == id);


        if (movie == null)
            return false;


        movie.IsArchived = false;

        movie.ArchivedAt = null;


        await _context.SaveChangesAsync();

        return true;
    }
}
