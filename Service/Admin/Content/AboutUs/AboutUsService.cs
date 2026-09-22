using DataBase.Context;
using Ganss.Xss;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Service.Extention.ImageExtention;
using System.Text.RegularExpressions;
namespace Service.Admin.Content.AboutUs;

public interface IAboutUsService
{
    Task<List<AboutUsListDto>> GetAllAsync();
    Task<AboutUsCreateDto> GetCreateDataAsync();
    Task<bool> CreateAsync(AboutUsCreateDto dto);
    Task<AboutUsEditDto?> GetEditDataAsync(int id);
    Task<bool> UpdateAsync(AboutUsEditDto dto);
    Task<AboutUsDetailsDto?> GetDetailsAsync(int id);
    Task<bool> ChangeStatusAsync(int id);
}

public class AboutUsService : IAboutUsService
{
    private readonly MyContext _context;
    private readonly IWebHostEnvironment _environment;

    public AboutUsService(
        MyContext context,
        IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    public async Task<List<AboutUsListDto>> GetAllAsync()
    {
        return await _context.AboutUs
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new AboutUsListDto
            {
                Id = x.Id,
                ImagePath = x.ImagePath,
                CustomerCount = x.CustomerCount,
                ActiveUserCount = x.ActiveUserCount,
                TotalVideoCount = x.TotalVideoCount,
                SubscriberCount = x.SubscriberCount,
                AwardCount = x.AwardCount,
                IsActive = x.IsActive,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            })
            .ToListAsync();
    }

    public Task<AboutUsCreateDto> GetCreateDataAsync()
    {
        var dto = new AboutUsCreateDto
        {
            IsActive = true
        };

        return Task.FromResult(dto);
    }

    public async Task<bool> CreateAsync(AboutUsCreateDto dto)
    {
        var exists = await _context.AboutUs
            .AnyAsync();

        if (exists)
            return false;

        if (dto.Image == null || dto.Image.Length == 0)
            return false;

        var imagePath =
            await dto.Image.SaveImageAsync(
                _environment.WebRootPath);

        if (string.IsNullOrWhiteSpace(imagePath))
            return false;

        var sanitizer = new HtmlSanitizer();

        var sanitizedContent =
            sanitizer.Sanitize(dto.Content);

        var plainText = Regex
            .Replace(
                sanitizedContent,
                "<.*?>",
                string.Empty)
            .Trim();

        if (string.IsNullOrWhiteSpace(plainText))
            return false;

        var aboutUs = new DataBase.Entity.AboutUs
        {
            ImagePath = imagePath,
            Content = sanitizedContent,
            CustomerCount = dto.CustomerCount,
            ActiveUserCount = dto.ActiveUserCount,
            TotalVideoCount = dto.TotalVideoCount,
            SubscriberCount = dto.SubscriberCount,
            AwardCount = dto.AwardCount,
            IsActive = dto.IsActive
        };

        _context.AboutUs.Add(aboutUs);

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<AboutUsEditDto?> GetEditDataAsync(int id)
    {
        return await _context.AboutUs
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new AboutUsEditDto
            {
                Id = x.Id,
                ExistingImagePath = x.ImagePath,
                Content = x.Content,
                CustomerCount = x.CustomerCount,
                ActiveUserCount = x.ActiveUserCount,
                TotalVideoCount = x.TotalVideoCount,
                SubscriberCount = x.SubscriberCount,
                AwardCount = x.AwardCount,
                IsActive = x.IsActive
            })
            .FirstOrDefaultAsync();
    }

    public async Task<bool> UpdateAsync(AboutUsEditDto dto)
    {
        var aboutUs = await _context.AboutUs
            .FirstOrDefaultAsync(x => x.Id == dto.Id);

        if (aboutUs == null)
            return false;

        var sanitizer = new HtmlSanitizer();

        var sanitizedContent =
            sanitizer.Sanitize(dto.Content);

        var plainText = Regex
            .Replace(
                sanitizedContent,
                "<.*?>",
                string.Empty)
            .Trim();

        if (string.IsNullOrWhiteSpace(plainText))
            return false;

        if (dto.Image != null && dto.Image.Length > 0)
        {
            var imagePath =
                await dto.Image.SaveImageAsync(
                    _environment.WebRootPath);

            if (!string.IsNullOrWhiteSpace(imagePath))
                aboutUs.ImagePath = imagePath;
        }

        aboutUs.Content = sanitizedContent;
        aboutUs.CustomerCount = dto.CustomerCount;
        aboutUs.ActiveUserCount = dto.ActiveUserCount;
        aboutUs.TotalVideoCount = dto.TotalVideoCount;
        aboutUs.SubscriberCount = dto.SubscriberCount;
        aboutUs.AwardCount = dto.AwardCount;
        aboutUs.IsActive = dto.IsActive;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<AboutUsDetailsDto?> GetDetailsAsync(int id)
    {
        return await _context.AboutUs
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new AboutUsDetailsDto
            {
                Id = x.Id,
                ImagePath = x.ImagePath,
                Content = x.Content,
                CustomerCount = x.CustomerCount,
                ActiveUserCount = x.ActiveUserCount,
                TotalVideoCount = x.TotalVideoCount,
                SubscriberCount = x.SubscriberCount,
                AwardCount = x.AwardCount,
                IsActive = x.IsActive,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            })
            .FirstOrDefaultAsync();
    }

    public async Task<bool> ChangeStatusAsync(int id)
    {
        var aboutUs = await _context.AboutUs
            .FirstOrDefaultAsync(x => x.Id == id);

        if (aboutUs == null)
            return false;

        aboutUs.IsActive = !aboutUs.IsActive;

        await _context.SaveChangesAsync();

        return true;
    }
}