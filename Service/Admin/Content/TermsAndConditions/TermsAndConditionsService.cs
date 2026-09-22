using DataBase.Context;
using Ganss.Xss;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
namespace Service.Admin.Content.TermsAndConditions;

public interface ITermsAndConditionsService
{
    Task<List<TermsAndConditionsListDto>> GetAllAsync();
    Task<TermsAndConditionsCreateDto> GetCreateDataAsync();
    Task<bool> CreateAsync(TermsAndConditionsCreateDto dto);
    Task<TermsAndConditionsEditDto?> GetEditDataAsync(int id);
    Task<bool> UpdateAsync(TermsAndConditionsEditDto dto);
    Task<TermsAndConditionsDetailsDto?> GetDetailsAsync(int id);
}

public class TermsAndConditionsService
    : ITermsAndConditionsService
{
    private readonly MyContext _context;

    public TermsAndConditionsService(MyContext context)
    {
        _context = context;
    }

    public async Task<List<TermsAndConditionsListDto>> GetAllAsync()
    {
        return await _context.TermsAndConditions
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new TermsAndConditionsListDto
            {
                Id = x.Id,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            })
            .ToListAsync();
    }

    public Task<TermsAndConditionsCreateDto> GetCreateDataAsync()
    {
        var dto = new TermsAndConditionsCreateDto();

        return Task.FromResult(dto);
    }

    public async Task<bool> CreateAsync(
        TermsAndConditionsCreateDto dto)
    {
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
        {
            return false;
        }

        var terms = new DataBase.Entity.TermsAndConditions
        {
            Content = sanitizedContent
        };

        _context.TermsAndConditions.Add(terms);

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<TermsAndConditionsEditDto?>
        GetEditDataAsync(int id)
    {
        return await _context.TermsAndConditions
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new TermsAndConditionsEditDto
            {
                Id = x.Id,
                Content = x.Content,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            })
            .FirstOrDefaultAsync();
    }

    public async Task<bool> UpdateAsync(
        TermsAndConditionsEditDto dto)
    {
        var terms = await _context.TermsAndConditions
            .FirstOrDefaultAsync(x => x.Id == dto.Id);

        if (terms == null)
        {
            return false;
        }

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
        {
            return false;
        }

        terms.Content = sanitizedContent;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<TermsAndConditionsDetailsDto?>
        GetDetailsAsync(int id)
    {
        return await _context.TermsAndConditions
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new TermsAndConditionsDetailsDto
            {
                Id = x.Id,
                Content = x.Content,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            })
            .FirstOrDefaultAsync();
    }
}