using DataBase.Context;
using Ganss.Xss;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
namespace Service.Admin.TvSeries.SeriesAdditionalInfo;

public interface ISeriesAdditionalInfoService
{
    Task<List<SeriesAdditionalInfoListDto>>GetAllAsync();

    Task<SeriesAdditionalInfoCreateDto>GetCreateDataAsync();

    Task FillCreateFormDataAsync(SeriesAdditionalInfoCreateDto dto);

    Task<bool> CreateAsync(SeriesAdditionalInfoCreateDto dto);

    Task<SeriesAdditionalInfoEditDto?>GetEditDataAsync(int id);

    Task<bool> UpdateAsync(SeriesAdditionalInfoEditDto dto);

    Task<SeriesAdditionalInfoDetailsDto?>GetDetailsAsync(int id);
}


public class SeriesAdditionalInfoService
    : ISeriesAdditionalInfoService
{
    private readonly MyContext _context;

    private readonly HtmlSanitizer _sanitizer;


    public SeriesAdditionalInfoService(
        MyContext context)
    {
        _context = context;

        _sanitizer = CreateSanitizer();
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
        var series = await _context.Series
            .AsNoTracking()
            .Where(x => !x.IsArchived)
            .Where(x =>
                !_context.MovieAdditionalInfos
                    .Any(info =>
                        info.MovieId == x.Id))
            .OrderBy(x => x.Title)
            .Select(x =>
                new SeriesAdditionalInfoSeriesItemDto
                {
                    Id = x.Id,

                    Title = x.Title
                })
            .ToListAsync();

        dto.Series = series;
    }


    // ==================================================
    // Create
    // ==================================================

    public async Task<bool> CreateAsync(
        SeriesAdditionalInfoCreateDto dto)
    {
        var seriesExists =
            await _context.Series
                .AnyAsync(x =>
                    x.Id == dto.SeriesId &&
                    !x.IsArchived);

        if (!seriesExists)
            return false;


        var additionalInfoExists =
            await _context.MovieAdditionalInfos
                .AnyAsync(x =>
                    x.MovieId == dto.SeriesId);

        if (additionalInfoExists)
            return false;


        var sanitizedContent =
            SanitizeContent(dto.Content);

        if (!HasTextContent(sanitizedContent))
            return false;


        var additionalInfo =
            new DataBase.Entity.MovieAdditionalInfo
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
    // Get Edit Data + SEO
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

                Content = x.Content,


                // ==============================
                // SEO
                // ==============================

                MetaTitle =x.Movie.MetaTitle,

                MetaDescription =x.Movie.MetaDescription,

                MetaKeywords =x.Movie.MetaKeywords,

                Slug =x.Movie.Slug,

                CanonicalUrl =x.Movie.CanonicalUrl,

                OgTitle =x.Movie.OgTitle,

                OgDescription =x.Movie.OgDescription,

                OgImage =x.Movie.OgImage,

                TwitterCard =x.Movie.TwitterCard,

                NoIndex =x.Movie.NoIndex,

                NoFollow =x.Movie.NoFollow
            })
            .FirstOrDefaultAsync();
    }


    // ==================================================
    // Update Content + SEO
    // ==================================================

    public async Task<bool> UpdateAsync(
        SeriesAdditionalInfoEditDto dto)
    {
        var additionalInfo =
            await _context.MovieAdditionalInfos
                .Include(x => x.Movie)
                .FirstOrDefaultAsync(x =>
                    x.Id == dto.Id &&
                    x.Movie is DataBase.Entity.Series);

        if (additionalInfo == null)
            return false;


        if (additionalInfo.Movie == null)
            return false;


        // ==================================================
        // بررسی سریال
        // ==================================================

        var seriesExists =
            await _context.Series
                .AnyAsync(x =>
                    x.Id == additionalInfo.MovieId &&
                    !x.IsArchived);

        if (!seriesExists)
            return false;


        // ==================================================
        // Sanitize Content
        // ==================================================

        var sanitizedContent =SanitizeContent(dto.Content);

        if (!HasTextContent(sanitizedContent))
            return false;


        // ==================================================
        // Slug
        // ==================================================

        var slug =
            string.IsNullOrWhiteSpace(dto.Slug)
                ? null
                : dto.Slug.Trim();


        if (!string.IsNullOrWhiteSpace(slug))
        {
            var duplicateSlug =
                await _context.Movies
                    .AnyAsync(x =>
                        x.Id != additionalInfo.MovieId &&
                        x.Slug == slug);

            if (duplicateSlug)
                return false;
        }


        // ==================================================
        // Update Content
        // ==================================================

        additionalInfo.Content =sanitizedContent;


        // ==================================================
        // Update SEO روی Movie
        // ==================================================

        additionalInfo.Movie.MetaTitle =
            NormalizeNullable(dto.MetaTitle);

        additionalInfo.Movie.MetaDescription =
            NormalizeNullable(dto.MetaDescription);

        additionalInfo.Movie.MetaKeywords =
            NormalizeNullable(dto.MetaKeywords);

        additionalInfo.Movie.Slug =slug;

        additionalInfo.Movie.CanonicalUrl =
            NormalizeNullable(dto.CanonicalUrl);

        additionalInfo.Movie.OgTitle =
            NormalizeNullable(dto.OgTitle);

        additionalInfo.Movie.OgDescription =
            NormalizeNullable(dto.OgDescription);

        additionalInfo.Movie.OgImage =
            NormalizeNullable(dto.OgImage);

        additionalInfo.Movie.TwitterCard =
            NormalizeTwitterCard(dto.TwitterCard);

        additionalInfo.Movie.NoIndex =dto.NoIndex;

        additionalInfo.Movie.NoFollow =dto.NoFollow;


        await _context.SaveChangesAsync();

        return true;
    }


    // ==================================================
    // Details
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

                UpdatedAt = x.UpdatedAt,


                // ==============================
                // SEO
                // ==============================

                MetaTitle =x.Movie.MetaTitle,

                MetaDescription =x.Movie.MetaDescription,

                MetaKeywords =x.Movie.MetaKeywords,

                Slug =x.Movie.Slug,

                CanonicalUrl =x.Movie.CanonicalUrl,

                OgTitle =x.Movie.OgTitle,

                OgDescription =x.Movie.OgDescription,

                OgImage =x.Movie.OgImage,

                TwitterCard =x.Movie.TwitterCard,

                NoIndex =x.Movie.NoIndex,

                NoFollow =x.Movie.NoFollow
            })
            .FirstOrDefaultAsync();
    }


    // ==================================================
    // Sanitize
    // ==================================================

    private string SanitizeContent(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            return string.Empty;

        return _sanitizer.Sanitize(content);
    }


    // ==================================================
    // Has Text
    // ==================================================

    private bool HasTextContent(
        string html)
    {
        var text =
            Regex.Replace(
                html,
                "<.*?>",
                string.Empty);

        return !string.IsNullOrWhiteSpace(text);
    }


    // ==================================================
    // Normalize Nullable
    // ==================================================

    private static string? NormalizeNullable(
        string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        return value.Trim();
    }


    // ==================================================
    // Twitter Card
    // ==================================================

    private static string? NormalizeTwitterCard(
        string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        var normalized =
            value.Trim().ToLowerInvariant();

        return normalized switch
        {
            "summary" => "summary",

            "summary_large_image" =>
                "summary_large_image",

            "app" => "app",

            "player" => "player",

            _ => null
        };
    }


    // ==================================================
    // Html Sanitizer
    // ==================================================

    private static HtmlSanitizer
        CreateSanitizer()
    {
        var sanitizer =
            new HtmlSanitizer();


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


        sanitizer.AllowedAttributes.Clear();

        sanitizer.AllowedAttributes.Add("href");
        sanitizer.AllowedAttributes.Add("src");
        sanitizer.AllowedAttributes.Add("alt");
        sanitizer.AllowedAttributes.Add("title");
        sanitizer.AllowedAttributes.Add("target");
        sanitizer.AllowedAttributes.Add("rel");
        sanitizer.AllowedAttributes.Add("class");


        sanitizer.AllowedClasses.Clear();

        sanitizer.AllowedClasses.Add(
            "ql-align-center");

        sanitizer.AllowedClasses.Add(
            "ql-align-right");

        sanitizer.AllowedClasses.Add(
            "ql-align-justify");


        sanitizer.AllowedSchemes.Clear();

        sanitizer.AllowedSchemes.Add("http");
        sanitizer.AllowedSchemes.Add("https");


        return sanitizer;
    }
}