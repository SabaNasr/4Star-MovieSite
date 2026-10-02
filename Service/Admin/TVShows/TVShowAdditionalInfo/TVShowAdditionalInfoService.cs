using DataBase.Context;
using Ganss.Xss;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
namespace Service.Admin.TVShows.TVShowAdditionalInfo;

public interface ITVShowAdditionalInfoService
{
    Task<List<TVShowAdditionalInfoListDto>>GetAllAsync();

    Task<TVShowAdditionalInfoCreateDto>GetCreateDataAsync();

    Task FillCreateFormDataAsync(TVShowAdditionalInfoCreateDto dto);

    Task<bool> CreateAsync(TVShowAdditionalInfoCreateDto dto);

    Task<TVShowAdditionalInfoEditDto?>GetEditDataAsync(int id);

    Task<bool> UpdateAsync(TVShowAdditionalInfoEditDto dto);

    Task<TVShowAdditionalInfoDetailsDto?>GetDetailsAsync(int id);
}


public class TVShowAdditionalInfoService: ITVShowAdditionalInfoService
{
    private readonly MyContext _context;

    private readonly HtmlSanitizer _sanitizer;


    public TVShowAdditionalInfoService(
        MyContext context)
    {
        _context = context;

        _sanitizer = CreateSanitizer();
    }


    // ==================================================
    // Get All
    // ==================================================

    public async Task<List<TVShowAdditionalInfoListDto>>GetAllAsync()
    {
        return await _context.MovieAdditionalInfos
            .AsNoTracking()
            .Where(x =>
                x.Movie is DataBase.Entity.TVShow)
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


    // ==================================================
    // Get Create Data
    // ==================================================

    public async Task<TVShowAdditionalInfoCreateDto>GetCreateDataAsync()
    {
        var dto =new TVShowAdditionalInfoCreateDto();

        await FillCreateFormDataAsync(dto);

        return dto;
    }


    // ==================================================
    // Fill Create Form Data
    // ==================================================

    public async Task FillCreateFormDataAsync(TVShowAdditionalInfoCreateDto dto)
    {
        var tvShows = await _context.Movies
            .AsNoTracking()
            .Where(x => !x.IsArchived)
            .Where(x =>
                x is DataBase.Entity.TVShow)
            .Where(x =>
                !_context.MovieAdditionalInfos
                    .Any(info =>
                        info.MovieId == x.Id))
            .OrderBy(x => x.Title)
            .Select(x =>
                new TVShowAdditionalInfoTVShowItemDto
                {
                    Id = x.Id,

                    Title = x.Title
                })
            .ToListAsync();

        dto.TVShows = tvShows;
    }


    // ==================================================
    // Create
    // ==================================================

    public async Task<bool> CreateAsync(TVShowAdditionalInfoCreateDto dto)
    {
        var tvShowExists =
            await _context.Movies
                .AnyAsync(x =>
                    x.Id == dto.TVShowId &&
                    !x.IsArchived &&
                    x is DataBase.Entity.TVShow);

        if (!tvShowExists)
            return false;


        var additionalInfoExists =
            await _context.MovieAdditionalInfos
                .AnyAsync(x =>
                    x.MovieId == dto.TVShowId);

        if (additionalInfoExists)
            return false;


        var sanitizedContent =SanitizeContent(dto.Content);

        if (!HasTextContent(sanitizedContent))
            return false;


        var additionalInfo =
            new DataBase.Entity.MovieAdditionalInfo
            {
                MovieId = dto.TVShowId,

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

    public async Task<TVShowAdditionalInfoEditDto?>GetEditDataAsync(int id)
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

    public async Task<bool> UpdateAsync(TVShowAdditionalInfoEditDto dto)
    {
        var additionalInfo =
            await _context.MovieAdditionalInfos
                .Include(x => x.Movie)
                .FirstOrDefaultAsync(x =>
                    x.Id == dto.Id &&
                    x.Movie is DataBase.Entity.TVShow);

        if (additionalInfo == null)
            return false;


        if (additionalInfo.Movie == null)
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
                ? null: dto.Slug.Trim();


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
        // Update SEO
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

    public async Task<TVShowAdditionalInfoDetailsDto?>GetDetailsAsync(int id)
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

    private bool HasTextContent(string html)
    {
        var text =
            Regex.Replace(
                html,
                "<.*?>",
                string.Empty);

        return !string.IsNullOrWhiteSpace(text);
    }


    // ==================================================
    // Normalize
    // ==================================================

    private static string? NormalizeNullable(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        return value.Trim();
    }


    // ==================================================
    // Twitter Card
    // ==================================================

    private static string? NormalizeTwitterCard(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        var normalized =value.Trim().ToLowerInvariant();

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

    private static HtmlSanitizer CreateSanitizer()
    {
        var sanitizer =new HtmlSanitizer();


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