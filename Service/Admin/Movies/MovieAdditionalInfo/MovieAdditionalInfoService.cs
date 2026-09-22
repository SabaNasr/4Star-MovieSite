using DataBase.Context;
using Ganss.Xss;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
namespace Service.Admin.Movies.MovieAdditionalInfo;

public interface IMovieAdditionalInfoService
{
    Task<List<MovieAdditionalInfoListDto>> GetAllAsync();

    Task<MovieAdditionalInfoCreateDto> GetCreateDataAsync();

    Task FillCreateFormDataAsync(MovieAdditionalInfoCreateDto dto);

    Task<bool> CreateAsync(MovieAdditionalInfoCreateDto dto);

    Task<MovieAdditionalInfoEditDto?> GetEditDataAsync(int id);

    Task<bool> UpdateAsync(MovieAdditionalInfoEditDto dto);

    Task<MovieAdditionalInfoDetailsDto?> GetDetailsAsync(int id);
}

public class MovieAdditionalInfoService : IMovieAdditionalInfoService
{
    private readonly MyContext _context;
    private readonly HtmlSanitizer _sanitizer;

    public MovieAdditionalInfoService(MyContext context)
    {
        _context = context;
        _sanitizer = CreateSanitizer();
    }

    public async Task<List<MovieAdditionalInfoListDto>> GetAllAsync()
    {
        return await _context.MovieAdditionalInfos
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new MovieAdditionalInfoListDto
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

    public async Task<MovieAdditionalInfoCreateDto> GetCreateDataAsync()
    {
        var dto = new MovieAdditionalInfoCreateDto();

        await FillCreateFormDataAsync(dto);

        return dto;
    }

    public async Task FillCreateFormDataAsync(
        MovieAdditionalInfoCreateDto dto)
    {
        var movies = await _context.Movies
            .AsNoTracking()
            .Where(x => !x.IsArchived)
            .Where(x => !_context.MovieAdditionalInfos
                .Any(info => info.MovieId == x.Id))
            .OrderBy(x => x.Title)
            .Select(x => new MovieAdditionalInfoMovieItemDto
            {
                Id = x.Id,
                Title = x.Title
            })
            .ToListAsync();

        dto.Movies = movies;
    }

    public async Task<bool> CreateAsync(
        MovieAdditionalInfoCreateDto dto)
    {
        var movieExists = await _context.Movies
            .AnyAsync(x =>
                x.Id == dto.MovieId &&
                !x.IsArchived);

        if (!movieExists)
            return false;

        var additionalInfoExists = await _context.MovieAdditionalInfos
            .AnyAsync(x => x.MovieId == dto.MovieId);

        if (additionalInfoExists)
            return false;

        var sanitizedContent = SanitizeContent(dto.Content);

        if (!HasTextContent(sanitizedContent))
            return false;

        var additionalInfo = new DataBase.Entity.MovieAdditionalInfo
        {
            MovieId = dto.MovieId,
            Content = sanitizedContent
        };

        _context.MovieAdditionalInfos.Add(additionalInfo);

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<MovieAdditionalInfoEditDto?> GetEditDataAsync(
        int id)
    {
        return await _context.MovieAdditionalInfos
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new MovieAdditionalInfoEditDto
            {
                Id = x.Id,
                MovieId = x.MovieId,
                MovieTitle = x.Movie.Title,
                Content = x.Content
            })
            .FirstOrDefaultAsync();
    }

    public async Task<bool> UpdateAsync(
        MovieAdditionalInfoEditDto dto)
    {
        var additionalInfo = await _context.MovieAdditionalInfos
            .FirstOrDefaultAsync(x => x.Id == dto.Id);

        if (additionalInfo == null)
            return false;

        var sanitizedContent = SanitizeContent(dto.Content);

        if (!HasTextContent(sanitizedContent))
            return false;

        additionalInfo.Content = sanitizedContent;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<MovieAdditionalInfoDetailsDto?> GetDetailsAsync(
        int id)
    {
        return await _context.MovieAdditionalInfos
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new MovieAdditionalInfoDetailsDto
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

    private string SanitizeContent(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            return string.Empty;

        return _sanitizer.Sanitize(content);
    }

    private bool HasTextContent(string html)
    {
        var text = Regex.Replace(
            html,
            "<.*?>",
            string.Empty);

        return !string.IsNullOrWhiteSpace(text);
    }

    private static HtmlSanitizer CreateSanitizer()
    {
        var sanitizer = new HtmlSanitizer();

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

        sanitizer.AllowedClasses.Add("ql-align-center");
        sanitizer.AllowedClasses.Add("ql-align-right");
        sanitizer.AllowedClasses.Add("ql-align-justify");

        sanitizer.AllowedSchemes.Clear();

        sanitizer.AllowedSchemes.Add("http");
        sanitizer.AllowedSchemes.Add("https");

        return sanitizer;
    }
}