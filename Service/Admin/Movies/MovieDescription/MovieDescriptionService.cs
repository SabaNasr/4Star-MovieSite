using DataBase.Context;
using Ganss.Xss;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Service.Admin.Movies.MovieDescription;

public interface IMovieDescriptionService
{
    Task<List<MovieDescriptionListDto>> GetAllAsync();

    Task<MovieDescriptionCreateDto> GetCreateDataAsync();

    Task FillCreateFormDataAsync(MovieDescriptionCreateDto dto);

    Task<bool> CreateAsync(MovieDescriptionCreateDto dto);

    Task<MovieDescriptionEditDto?> GetEditDataAsync(int id);

    Task<bool> UpdateAsync(MovieDescriptionEditDto dto);

    Task<MovieDescriptionDetailsDto?> GetDetailsAsync(int id);
}


public class MovieDescriptionService : IMovieDescriptionService
{
    private readonly MyContext _context;

    private readonly HtmlSanitizer _sanitizer;


    public MovieDescriptionService(MyContext context)
    {
        _context = context;

        _sanitizer = CreateSanitizer();
    }


    // ==================================================
    // دریافت تمام شرح فیلم‌ها
    // ==================================================

    public async Task<List<MovieDescriptionListDto>> GetAllAsync()
    {
        return await _context.MovieDescriptions
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new MovieDescriptionListDto
            {
                Id = x.Id,

                MovieId = x.MovieId,

                MovieTitle = x.Movie.Title,

                IsActive = x.IsActive,

                CreatedAt = x.CreatedAt,

                UpdatedAt = x.UpdatedAt
            })
            .ToListAsync();
    }


    // ==================================================
    // دریافت اطلاعات اولیه فرم Create
    // ==================================================

    public async Task<MovieDescriptionCreateDto> GetCreateDataAsync()
    {
        var dto = new MovieDescriptionCreateDto();

        await FillCreateFormDataAsync(dto);

        return dto;
    }


    // ==================================================
    // پر کردن لیست فیلم‌های قابل انتخاب
    // ==================================================

    public async Task FillCreateFormDataAsync(
        MovieDescriptionCreateDto dto)
    {
        var movies = await _context.Movies
            .AsNoTracking()
            .Where(x => !x.IsArchived)
            .Where(x => !_context.MovieDescriptions
                .Any(description => description.MovieId == x.Id))
            .OrderBy(x => x.Title)
            .Select(x => new MovieDescriptionMovieItemDto
            {
                Id = x.Id,

                Title = x.Title
            })
            .ToListAsync();


        dto.Movies = movies;
    }


    // ==================================================
    // ایجاد شرح کامل فیلم
    // ==================================================

    public async Task<bool> CreateAsync(
        MovieDescriptionCreateDto dto)
    {
        var movieExists = await _context.Movies
            .AnyAsync(x =>
                x.Id == dto.MovieId &&
                !x.IsArchived);

        if (!movieExists)
            return false;


        var descriptionExists = await _context.MovieDescriptions
            .AnyAsync(x => x.MovieId == dto.MovieId);

        if (descriptionExists)
            return false;


        var sanitizedContent = SanitizeContent(dto.Content);

        if (!HasTextContent(sanitizedContent))
            return false;


        var description = new DataBase.Entity.MovieDescription
        {
            MovieId = dto.MovieId,

            Content = sanitizedContent
        };


        _context.MovieDescriptions.Add(description);

        await _context.SaveChangesAsync();

        return true;
    }


    // ==================================================
    // دریافت اطلاعات Description برای Edit
    // ==================================================

    public async Task<MovieDescriptionEditDto?> GetEditDataAsync(
        int id)
    {
        return await _context.MovieDescriptions
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new MovieDescriptionEditDto
            {
                Id = x.Id,

                MovieId = x.MovieId,

                MovieTitle = x.Movie.Title,

                Content = x.Content
            })
            .FirstOrDefaultAsync();
    }


    // ==================================================
    // ویرایش Description
    // ==================================================

    public async Task<bool> UpdateAsync(
        MovieDescriptionEditDto dto)
    {
        var description = await _context.MovieDescriptions
            .FirstOrDefaultAsync(x => x.Id == dto.Id);

        if (description == null)
            return false;


        var sanitizedContent = SanitizeContent(dto.Content);

        if (!HasTextContent(sanitizedContent))
            return false;


        description.Content = sanitizedContent;

        await _context.SaveChangesAsync();

        return true;
    }


    // ==================================================
    // دریافت جزئیات Description
    // ==================================================

    public async Task<MovieDescriptionDetailsDto?> GetDetailsAsync(
        int id)
    {
        return await _context.MovieDescriptions
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new MovieDescriptionDetailsDto
            {
                Id = x.Id,

                MovieId = x.MovieId,

                MovieTitle = x.Movie.Title,

                Content = x.Content,

                IsActive = x.IsActive,

                CreatedAt = x.CreatedAt,

                UpdatedAt = x.UpdatedAt
            })
            .FirstOrDefaultAsync();
    }


    // ==================================================
    // پاک‌سازی HTML
    // ==================================================

    private string SanitizeContent(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            return string.Empty;


        return _sanitizer.Sanitize(content);
    }


    // ==================================================
    // بررسی وجود محتوای متنی واقعی
    // ==================================================

    private bool HasTextContent(string html)
    {
        var text = Regex.Replace(
            html,
            "<.*?>",
            string.Empty);

        return !string.IsNullOrWhiteSpace(text);
    }


    // ==================================================
    // ساخت HtmlSanitizer
    // ==================================================

    private static HtmlSanitizer CreateSanitizer()
    {
        var sanitizer = new HtmlSanitizer();


        // ------------------------------------------------
        // تگ‌های مجاز
        // ------------------------------------------------

        sanitizer.AllowedTags.Clear();

        sanitizer.AllowedTags.Add("p");

        sanitizer.AllowedTags.Add("br");

        sanitizer.AllowedTags.Add("h1");
        sanitizer.AllowedTags.Add("h2");
        sanitizer.AllowedTags.Add("h3");
        sanitizer.AllowedTags.Add("h4");
        sanitizer.AllowedTags.Add("h5");
        sanitizer.AllowedTags.Add("h6");

        sanitizer.AllowedTags.Add("strong");
        sanitizer.AllowedTags.Add("em");
        sanitizer.AllowedTags.Add("u");

        sanitizer.AllowedTags.Add("ol");
        sanitizer.AllowedTags.Add("ul");
        sanitizer.AllowedTags.Add("li");

        sanitizer.AllowedTags.Add("a");

        sanitizer.AllowedTags.Add("img");

        sanitizer.AllowedTags.Add("blockquote");


        // ------------------------------------------------
        // Attributeهای مجاز
        // ------------------------------------------------

        sanitizer.AllowedAttributes.Clear();

        sanitizer.AllowedAttributes.Add("href");

        sanitizer.AllowedAttributes.Add("src");

        sanitizer.AllowedAttributes.Add("alt");

        sanitizer.AllowedAttributes.Add("title");

        sanitizer.AllowedAttributes.Add("target");

        sanitizer.AllowedAttributes.Add("rel");

        sanitizer.AllowedAttributes.Add("class");


        // ------------------------------------------------
        // فقط Classهای موردنیاز Quill
        // ------------------------------------------------

        sanitizer.AllowedClasses.Clear();

        sanitizer.AllowedClasses.Add("ql-align-center");

        sanitizer.AllowedClasses.Add("ql-align-right");

        sanitizer.AllowedClasses.Add("ql-align-justify");


        // ------------------------------------------------
        // Schemeهای مجاز برای لینک و تصویر
        // ------------------------------------------------

        sanitizer.AllowedSchemes.Clear();

        sanitizer.AllowedSchemes.Add("http");

        sanitizer.AllowedSchemes.Add("https");


        return sanitizer;
    }
}