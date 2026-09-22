using DataBase.Context;
using DataBase.Entity;
using Ganss.Xss;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
namespace Service.Admin.Content.PrivacyPolicies;

public interface IPrivacyPolicyService
{
    Task<List<PrivacyPolicyListDto>> GetAllAsync();

    Task<PrivacyPolicyCreateDto> GetCreateDataAsync();

    Task<bool> CreateAsync(PrivacyPolicyCreateDto dto);

    Task<PrivacyPolicyEditDto?> GetEditDataAsync(int id);

    Task<bool> UpdateAsync(PrivacyPolicyEditDto dto);

    Task<PrivacyPolicyDetailsDto?> GetDetailsAsync(int id);
}

public class PrivacyPolicyService : IPrivacyPolicyService
{
    private readonly MyContext _context;

    public PrivacyPolicyService(MyContext context)
    {
        _context = context;
    }

    public async Task<List<PrivacyPolicyListDto>> GetAllAsync()
    {
        return await _context.Set<PrivacyPolicy>()
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new PrivacyPolicyListDto
            {
                Id = x.Id,
                IsActive = x.IsActive,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            })
            .ToListAsync();
    }

    public Task<PrivacyPolicyCreateDto> GetCreateDataAsync()
    {
        var dto = new PrivacyPolicyCreateDto();

        return Task.FromResult(dto);
    }

    public async Task<bool> CreateAsync(
        PrivacyPolicyCreateDto dto)
    {
        var exists = await _context.Set<PrivacyPolicy>()
            .AnyAsync();

        if (exists)
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

        var privacyPolicy = new PrivacyPolicy
        {
            Content = sanitizedContent
        };

        _context.Set<PrivacyPolicy>()
            .Add(privacyPolicy);

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<PrivacyPolicyEditDto?> GetEditDataAsync(
        int id)
    {
        return await _context.Set<PrivacyPolicy>()
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new PrivacyPolicyEditDto
            {
                Id = x.Id,
                Content = x.Content
            })
            .FirstOrDefaultAsync();
    }

    public async Task<bool> UpdateAsync(
        PrivacyPolicyEditDto dto)
    {
        var privacyPolicy =
            await _context.Set<PrivacyPolicy>()
                .FirstOrDefaultAsync(x => x.Id == dto.Id);

        if (privacyPolicy == null)
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

        privacyPolicy.Content = sanitizedContent;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<PrivacyPolicyDetailsDto?> GetDetailsAsync(
        int id)
    {
        return await _context.Set<PrivacyPolicy>()
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new PrivacyPolicyDetailsDto
            {
                Id = x.Id,
                Content = x.Content,
                IsActive = x.IsActive,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            })
            .FirstOrDefaultAsync();
    }
}
