using DataBase.Context;
using Microsoft.EntityFrameworkCore;
namespace Service.Admin.Movies.Language;

public interface ILanguageService
{
    Task<List<LanguageListDto>> GetAllAsync();
    Task<LanguageEditDto?> GetEditDataAsync(int id);
    Task<bool> CreateAsync(LanguageCreateDto dto);
    Task<bool> UpdateAsync(LanguageEditDto dto);
    Task<bool> ChangeStatusAsync(int id);
}

public class LanguageService : ILanguageService
{
    private readonly MyContext _context;

    public LanguageService(MyContext context)
    {
        _context = context;
    }

    // ==================================================
    // Get All Languages
    // ==================================================

    public async Task<List<LanguageListDto>> GetAllAsync()
    {
        return await _context.Languages
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .Select(x => new LanguageListDto
            {
                Id = x.Id,
                Name = x.Name,
                IsActive = x.IsActive,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            })
            .ToListAsync();
    }

    // ==================================================
    // Get Language For Edit
    // ==================================================

    public async Task<LanguageEditDto?> GetEditDataAsync(int id)
    {
        return await _context.Languages
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new LanguageEditDto
            {
                Id = x.Id,
                Name = x.Name
            })
            .FirstOrDefaultAsync();
    }

    // ==================================================
    // Create Language
    // ==================================================

    public async Task<bool> CreateAsync(LanguageCreateDto dto)
    {
        var name = dto.Name.Trim();

        var exists = await _context.Languages
            .AnyAsync(x => x.Name == name);

        if (exists)
            return false;

        var language = new DataBase.Entity.Language
        {
            Name = name,
            IsActive = true
        };

        _context.Languages.Add(language);

        await _context.SaveChangesAsync();

        return true;
    }

    // ==================================================
    // Update Language
    // ==================================================

    public async Task<bool> UpdateAsync(LanguageEditDto dto)
    {
        var language = await _context.Languages
            .FirstOrDefaultAsync(x => x.Id == dto.Id);

        if (language == null)
            return false;

        var name = dto.Name.Trim();

        var exists = await _context.Languages
            .AnyAsync(x => x.Id != dto.Id && x.Name == name);

        if (exists)
            return false;

        language.Name = name;

        await _context.SaveChangesAsync();

        return true;
    }

    // ==================================================
    // Change Language Status
    // ==================================================

    public async Task<bool> ChangeStatusAsync(int id)
    {
        var language = await _context.Languages
            .FirstOrDefaultAsync(x => x.Id == id);

        if (language == null)
            return false;

        language.IsActive = !language.IsActive;

        await _context.SaveChangesAsync();

        return true;
    }
}