using DataBase.Context;
using Ganss.Xss;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Service.Extention.ImageExtention;
using System.Text.RegularExpressions;
namespace Service.Admin.Blog.BlogPost;

public interface IBlogPostService
{
    Task<List<BlogPostListDto>> GetAllAsync();

    Task<BlogPostCreateDto> GetCreateDataAsync();

    Task FillCreateFormDataAsync(BlogPostCreateDto dto);

    Task<bool> CreateAsync(BlogPostCreateDto dto);

    Task<BlogPostEditDto?> GetEditDataAsync(int id);

    Task FillEditFormDataAsync(BlogPostEditDto dto);

    Task<bool> UpdateAsync(BlogPostEditDto dto);

    Task<BlogPostDetailsDto?> GetDetailsAsync(int id);

    Task<bool> DeleteAsync(int id);

    Task<bool> ChangeStatusAsync(int id);
}

public class BlogPostService : IBlogPostService
{
    private readonly MyContext _context;
    private readonly IWebHostEnvironment _environment;
    private readonly HtmlSanitizer _sanitizer;

    public BlogPostService(
        MyContext context,
        IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
        _sanitizer = CreateSanitizer();
    }

    public async Task<List<BlogPostListDto>> GetAllAsync()
    {
        return await _context.BlogPosts
            .AsNoTracking()
            .OrderByDescending(x => x.PublishDate)
            .Select(x => new BlogPostListDto
            {
                Id = x.Id,
                Title = x.Title,
                Slug = x.Slug,
                ImagePath = x.ImagePath,
                PublishDate = x.PublishDate,
                LikeCount = x.LikeCount,
                IsActive = x.IsActive,
                IsArchived = x.IsArchived,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            })
            .ToListAsync();
    }

    public async Task<BlogPostCreateDto> GetCreateDataAsync()
    {
        var dto = new BlogPostCreateDto();

        await FillCreateFormDataAsync(dto);

        return dto;
    }

    public async Task FillCreateFormDataAsync(BlogPostCreateDto dto)
    {
        dto.Categories = await _context.BlogCategories
            .AsNoTracking()
            .Where(x => x.IsActive)
            .OrderBy(x => x.DisplayOrder)
            .ThenBy(x => x.Name)
            .Select(x => new BlogPostCategoryItemDto
            {
                Id = x.Id,
                Name = x.Name,
                IsSelected = dto.SelectedCategoryIds.Contains(x.Id)
            })
            .ToListAsync();
    }

    public async Task<bool> CreateAsync(BlogPostCreateDto dto)
    {
        var slugExists = await _context.BlogPosts
            .AnyAsync(x => x.Slug == dto.Slug.Trim());

        if (slugExists)
            return false;

        if (dto.Image == null || dto.Image.Length == 0)
            return false;

        var sanitizedContent = SanitizeContent(dto.Content);

        if (!HasTextContent(sanitizedContent))
            return false;

        var imagePath = await dto.Image.SaveImageAsync(
            _environment.WebRootPath,
            "uploads",
            "blog");

        if (string.IsNullOrWhiteSpace(imagePath))
            return false;

        var categories = await _context.BlogCategories
            .Where(x => dto.SelectedCategoryIds.Contains(x.Id))
            .ToListAsync();

        var blogPost = new DataBase.Entity.BlogPost
        {
            Title = dto.Title.Trim(),

            Slug = dto.Slug.Trim(),

            ImagePath = imagePath,

            ShortDescription = dto.ShortDescription.Trim(),

            Content = sanitizedContent,

            PublishDate = dto.PublishDate,

            IsActive = dto.IsActive,

            LikeCount = 0,

            Categories = categories
        };

        _context.BlogPosts.Add(blogPost);

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<BlogPostEditDto?> GetEditDataAsync(int id)
    {
        var blogPost = await _context.BlogPosts
            .AsNoTracking()
            .Include(x => x.Categories)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (blogPost == null)
            return null;

        var dto = new BlogPostEditDto
        {
            Id = blogPost.Id,

            Title = blogPost.Title,

            Slug = blogPost.Slug,

            ExistingImagePath = blogPost.ImagePath,

            ShortDescription = blogPost.ShortDescription,

            Content = blogPost.Content,

            PublishDate = blogPost.PublishDate,

            IsActive = blogPost.IsActive,

            SelectedCategoryIds = blogPost.Categories
                .Select(x => x.Id)
                .ToList()
        };

        await FillEditFormDataAsync(dto);

        return dto;
    }

    public async Task FillEditFormDataAsync(BlogPostEditDto dto)
    {
        dto.Categories = await _context.BlogCategories
            .AsNoTracking()
            .Where(x => x.IsActive || dto.SelectedCategoryIds.Contains(x.Id))
            .OrderBy(x => x.DisplayOrder)
            .ThenBy(x => x.Name)
            .Select(x => new BlogPostCategoryItemDto
            {
                Id = x.Id,
                Name = x.Name,
                IsSelected = dto.SelectedCategoryIds.Contains(x.Id)
            })
            .ToListAsync();
    }

    public async Task<bool> UpdateAsync(BlogPostEditDto dto)
    {
        var blogPost = await _context.BlogPosts
            .Include(x => x.Categories)
            .FirstOrDefaultAsync(x => x.Id == dto.Id);

        if (blogPost == null)
            return false;

        var slug = dto.Slug.Trim();

        var slugExists = await _context.BlogPosts
            .AnyAsync(x =>
                x.Id != dto.Id &&
                x.Slug == slug);

        if (slugExists)
            return false;

        var sanitizedContent = SanitizeContent(dto.Content);

        if (!HasTextContent(sanitizedContent))
            return false;

        blogPost.Title = dto.Title.Trim();

        blogPost.Slug = slug;

        blogPost.ShortDescription = dto.ShortDescription.Trim();

        blogPost.Content = sanitizedContent;

        blogPost.PublishDate = dto.PublishDate;

        blogPost.IsActive = dto.IsActive;

        if (dto.Image != null && dto.Image.Length > 0)
        {
            var imagePath = await dto.Image.SaveImageAsync(
                _environment.WebRootPath,
                "uploads",
                "blog");

            if (!string.IsNullOrWhiteSpace(imagePath))
            {
                blogPost.ImagePath = imagePath;
            }
        }

        blogPost.Categories.Clear();

        if (dto.SelectedCategoryIds.Count > 0)
        {
            var categories = await _context.BlogCategories
                .Where(x => dto.SelectedCategoryIds.Contains(x.Id))
                .ToListAsync();

            foreach (var category in categories)
            {
                blogPost.Categories.Add(category);
            }
        }

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<BlogPostDetailsDto?> GetDetailsAsync(int id)
    {
        return await _context.BlogPosts
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new BlogPostDetailsDto
            {
                Id = x.Id,

                Title = x.Title,

                Slug = x.Slug,

                ImagePath = x.ImagePath,

                ShortDescription = x.ShortDescription,

                Content = x.Content,

                PublishDate = x.PublishDate,

                LikeCount = x.LikeCount,

                IsActive = x.IsActive,

                IsArchived = x.IsArchived,

                Categories = x.Categories
                    .Select(category => category.Name)
                    .ToList(),

                CreatedAt = x.CreatedAt,

                UpdatedAt = x.UpdatedAt
            })
            .FirstOrDefaultAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var blogPost = await _context.BlogPosts
            .FirstOrDefaultAsync(x => x.Id == id);

        if (blogPost == null)
            return false;

        _context.BlogPosts.Remove(blogPost);

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> ChangeStatusAsync(int id)
    {
        var blogPost = await _context.BlogPosts
            .FirstOrDefaultAsync(x => x.Id == id);

        if (blogPost == null)
            return false;

        blogPost.IsActive = !blogPost.IsActive;

        await _context.SaveChangesAsync();

        return true;
    }

    private string SanitizeContent(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            return string.Empty;

        return _sanitizer.Sanitize(content);
    }

    private bool HasTextContent(string html)
    {
        var text = Regex.Replace(html, "<.*?>", string.Empty);

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
